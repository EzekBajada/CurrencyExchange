namespace CurrencyExchange.Services;

public class ExchangeCurrencyService : IExchangeCurrencyService
{
    private readonly IFixerIoService _fixerIoService;
    private readonly ILogger<ExchangeCurrencyService> _logger;
    private readonly IRedisService _redisService;
    private readonly IRepository<Client> _clientRepository;
    private readonly IRepository<CurrencyExchangeHistory> _currencyExchangeHistoryRepository;
    private readonly AppSettings _appSettings;

    public ExchangeCurrencyService(IFixerIoService fixerIoService, 
        ILogger<ExchangeCurrencyService> logger, 
        IRedisService redisService,
        IRepository<Client> clientRepository,
        IRepository<CurrencyExchangeHistory> currencyExchangeHistoryRepository,
        AppSettings appSettings)
    {
        _fixerIoService = fixerIoService;
        _logger = logger;
        _redisService = redisService;
        _clientRepository = clientRepository;
        _currencyExchangeHistoryRepository = currencyExchangeHistoryRepository;
        _appSettings = appSettings;
    }

    public async Task<ExchangeCurrencyResponse> ExchangeCurrenciesAsync(ExchangeCurrencyRequest request)
    { 
        try
        {
            // Check if client exists
            var client = await _clientRepository.GetOneByIdAsync(request.ClientId ?? default);
            if (client is null)
            {
                _logger.LogError(InfoErrorMessages.ClientNotFound(request.ClientId));
                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = InfoErrorMessages.ClientNotFound(request.ClientId)
                };
            }

            // Get exchange trades history in the last hour
            var clientExchangeHistory =
                await _currencyExchangeHistoryRepository.GetMultipleByFilterAsync(
                    row => row.ClientId == request.ClientId && row.ExecutedDate >= DateTime.UtcNow.AddHours(-1));
            
            if (clientExchangeHistory?.Count() > _appSettings.MaxAllowedRequests)
            {
                _logger.LogError(InfoErrorMessages.ExceededAllowedRequests(client.ClientName));
                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = InfoErrorMessages.ExceededAllowedRequests(client.ClientName)
                };
            }
            
            // Check if exchange rate is cached
            var cachedExchangeRate = await _redisService.GetAsync<LatestExchangeRatesResponse>($"{request.FromCurrency}_{request.ToCurrency}")!;

            LatestExchangeRatesResponse? currencyRate;

            if (cachedExchangeRate != null &&
                DateTime.UnixEpoch.AddSeconds(cachedExchangeRate.TimeStamp).ToLocalTime() >= DateTime.Now.AddMinutes(-30))
            {
                currencyRate = cachedExchangeRate;
            }
            else
            {
                // If not cached get it from FixerIo
                currencyRate = await _fixerIoService.GetCurrencyExchangeRateAsync(request.FromCurrency ?? client?.BaseCurrency, request.ToCurrency);
                
                if(!await _redisService.PutAsync($"{request.FromCurrency}_{request.ToCurrency}",
                       JsonSerializer.Serialize(currencyRate)))
                {
                    _logger.LogError(InfoErrorMessages.CurrencyRateCacheError);
                }
            }
            
            // If success false means that the rate is not cached and fixerIo returned false
            if (currencyRate is {Success: false})
            {
                var message = currencyRate.ErrorMessage ?? InfoErrorMessages.FixerIoFetchError;
                _logger.LogError(message);
                
                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = message
                };
            }

            var rate = currencyRate?.Rates!.FirstOrDefault();
            var amountConverted = request.Amount * rate?.Value;

            // Add in history table
            await _currencyExchangeHistoryRepository.AddOneAsync(new CurrencyExchangeHistory
            {
                ClientId = request?.ClientId,
                FromCurrency = request?.FromCurrency,
                ToCurrency = rate?.Key,
                ExchangeRate = rate?.Value,
                AmountIn = request?.Amount,
                AmountOut = amountConverted,
                ExecutedDate = DateTime.UtcNow
            });
            
            await _currencyExchangeHistoryRepository.SaveDbChangesAsync();

            return new ExchangeCurrencyResponse
            {
                ClientId = request?.ClientId,
                CurrencyConverted = new ConvertedCurrency(request?.Amount * currencyRate?.Rates!.FirstOrDefault().Value, request?.FromCurrency!, rate?.Key, rate?.Value),
                Success = true
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, InfoErrorMessages.ExchangeRateGeneralError);
            return new ExchangeCurrencyResponse
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }
    }
}