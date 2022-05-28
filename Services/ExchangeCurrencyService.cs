namespace CurrencyExchange.Services;

public class ExchangeCurrencyService : IExchangeCurrencyService
{
    private readonly CurrencyExchangeDbContext _dbContext;
    private readonly IFixerIoService _fixerIoService;
    private readonly ILogger<ExchangeCurrencyService> _logger;
    private readonly IRedisService _redisService;

    public ExchangeCurrencyService(CurrencyExchangeDbContext dbContext, IFixerIoService fixerIoService, ILogger<ExchangeCurrencyService> logger, IRedisService redisService)
    {
        _dbContext = dbContext;
        _fixerIoService = fixerIoService;
        _logger = logger;
        _redisService = redisService;
    }

    public ExchangeCurrencyResponse ExchangeCurrencies(ExchangeCurrencyRequest request)
    {
        try
        {
            var client = _dbContext.Clients?.FirstOrDefault(x=> x.ClientId == request.ClientId);
            if (client is null)
            {
                const string message = "Client does not exist";
                LoggingUtilities<ExchangeCurrencyService>.LogInformation(message, _logger, true);

                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = message
                };
            }
            
            var clientExchangeHistory = _dbContext.CurrencyExchangeHistories?
                .Where(x => x.ClientId == request.ClientId
                    && x.ExecutedDate >= DateTime.Now.AddHours(-1))
                .ToList();

            if (clientExchangeHistory is {Count: >= 10})
            {
                const string message = "Exceeded maximum exchanges for 1 hour";
                LoggingUtilities<ExchangeCurrencyService>.LogInformation(message, _logger, true);

                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = message
                };
            }
            
            var cachedExchangeRate = _redisService.Get<LatestExchangeRatesResponse>($"{request.FromCurrency}_{request.ToCurrency}");

            LatestExchangeRatesResponse? currencyRate;

            if (cachedExchangeRate != null &&
                DateTime.UnixEpoch.AddSeconds(cachedExchangeRate.TimeStamp).ToLocalTime() >= DateTime.Now.AddMinutes(-30))
            {
                currencyRate = cachedExchangeRate;
            }
            else
            {
                currencyRate = _fixerIoService.GetCurrencyExchangeRate(request.FromCurrency ?? client?.BaseCurrency, request.ToCurrency).Result;
                
                if(!_redisService.Put($"{request.FromCurrency}_{request.ToCurrency}",
                       JsonSerializer.Serialize(currencyRate)))
                {
                    LoggingUtilities<ExchangeCurrencyService>.LogInformation("Currency rate could not be cached", _logger, true);
                }
            }
            
            if (currencyRate is not {Success: true})
            {
                var message = currencyRate?.ErrorMessage ?? "Currency rate could not be fetched: FixerIo error";
                LoggingUtilities<ExchangeCurrencyService>.LogInformation(message, _logger, true);

                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = message
                };
            }

            var rate = currencyRate.Rates!.FirstOrDefault();
            var amountConverted = request.Amount * rate.Value;

            // Add in history table
            _dbContext.CurrencyExchangeHistories?.Add(new CurrencyExchangeHistory
            {
                ClientId = request?.ClientId,
                FromCurrency = request?.FromCurrency,
                ToCurrency = rate.Key,
                ExchangeRate = rate.Value,
                AmountIn = request?.Amount,
                AmountOut = amountConverted,
                ExecutedDate = DateTime.UtcNow
            });

            _dbContext.SaveChanges();

            return new ExchangeCurrencyResponse
            {
                ClientId = request?.ClientId,
                CurrencyConverted = new ConvertedCurrency(request?.Amount * currencyRate.Rates!.FirstOrDefault().Value, request?.FromCurrency!, rate.Key, rate.Value),
                Success = true
            };
        }
        catch (Exception e)
        {
            LoggingUtilities<ExchangeCurrencyService>.LogInformation(e.ToString(), _logger, true);

            return new ExchangeCurrencyResponse
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }
    }
}