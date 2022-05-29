using CurrencyExchange.Interfaces.RepositoryInterfaces;
using CurrencyExchange.Models.Configurations;

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

    public async Task<ExchangeCurrencyResponse> ExchangeCurrencies(ExchangeCurrencyRequest request)
    { 
        try
        {
            var client = await _clientRepository.GetOneById(request.ClientId ?? default);
            if (client is null)
            {
                _logger.LogError(ErrorMessages.ClientNotFound(request.ClientId));
                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.ClientNotFound(request.ClientId)
                };
            }

            var clientExchangeHistory =
                await _currencyExchangeHistoryRepository.GetMultipleByFilter(
                    row => row.ClientId == request.ClientId && row.ExecutedDate >= DateTime.Now.AddHours(-1));
            
            if (clientExchangeHistory?.Count() > _appSettings.MaxAllowedRequests)
            {
                _logger.LogError(ErrorMessages.ExceededAllowedRequests(client.ClientName));
                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.ExceededAllowedRequests(client.ClientName)
                };
            }
            
            var cachedExchangeRate = await _redisService.Get<LatestExchangeRatesResponse>($"{request.FromCurrency}_{request.ToCurrency}")!;

            LatestExchangeRatesResponse? currencyRate;

            if (cachedExchangeRate != null &&
                DateTime.UnixEpoch.AddSeconds(cachedExchangeRate.TimeStamp).ToLocalTime() >= DateTime.Now.AddMinutes(-30))
            {
                currencyRate = cachedExchangeRate;
            }
            else
            {
                currencyRate = _fixerIoService.GetCurrencyExchangeRate(request.FromCurrency ?? client?.BaseCurrency, request.ToCurrency).Result;
                
                if(!await _redisService.Put($"{request.FromCurrency}_{request.ToCurrency}",
                       JsonSerializer.Serialize(currencyRate)))
                {
                    _logger.LogError(ErrorMessages.CurrencyRateCacheError);
                }
            }
            
            // If success false means that the rate is not cached and fixerIo returned false
            if (currencyRate is {Success: false})
            {
                var message = currencyRate?.ErrorMessage ?? ErrorMessages.FixerIoFetchError;
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
            await _currencyExchangeHistoryRepository.AddOne(new CurrencyExchangeHistory
            {
                ClientId = request?.ClientId,
                FromCurrency = request?.FromCurrency,
                ToCurrency = rate?.Key,
                ExchangeRate = rate?.Value,
                AmountIn = request?.Amount,
                AmountOut = amountConverted,
                ExecutedDate = DateTime.UtcNow
            });
            
            await _currencyExchangeHistoryRepository.SaveDbChanges();

            return new ExchangeCurrencyResponse
            {
                ClientId = request?.ClientId,
                CurrencyConverted = new ConvertedCurrency(request?.Amount * currencyRate?.Rates!.FirstOrDefault().Value, request?.FromCurrency!, rate?.Key, rate?.Value),
                Success = true
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorMessages.ExchangeRateGeneralError);
            return new ExchangeCurrencyResponse
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }
    }
}