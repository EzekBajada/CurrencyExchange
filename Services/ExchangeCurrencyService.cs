namespace CurrencyExchange.Services;

public class ExchangeCurrencyService : IExchangeCurrencyService
{
    private readonly CurrencyExchangeDbContext _dbContext;
    private readonly IFixerIoService _fixerIoService;
    private readonly ILogger<ExchangeCurrencyService> _logger;

    public ExchangeCurrencyService(CurrencyExchangeDbContext dbContext, IFixerIoService fixerIoService, ILogger<ExchangeCurrencyService> logger)
    {
        _dbContext = dbContext;
        _fixerIoService = fixerIoService;
        _logger = logger;
    }

    public ExchangeCurrencyResponse ExchangeCurrencies(ExchangeCurrencyRequest request)
    {
        try
        {
            var client = _dbContext?.Clients?.Where(x => x.ClientId == request.ClientId).FirstOrDefault();

            var currencyRate = _fixerIoService.GetCurrencyExchangeRate(request?.FromCurrency ?? client?.BaseCurrency, request?.ToCurrencies).Result;

            if ((currencyRate != null && !currencyRate.Success))
            {
                var message = currencyRate.ErrorMessage ?? "Currency rate could not be fetched: FixerIo error";
                LoggingUtilities<ExchangeCurrencyService>.LogInformation(message, _logger, true);

                return new ExchangeCurrencyResponse
                {
                    Success = false,
                    ErrorMessage = message
                };
            }

            var convertedCurrencies = new List<ConvertedCurrency>();
            foreach (var rate in currencyRate?.Rates!)
            {
                var amountConverted = request?.Amount * rate.Value;

                convertedCurrencies.Add(new ConvertedCurrency(request?.Amount * rate.Value, request?.FromCurrency!, rate.Key, rate.Value));

                // Add in history table
                _dbContext?.CurrencyExchangeHistories?.Add(new CurrencyExchangeHistory
                {
                    ClientId = request?.ClientId,
                    FromCurrency = request?.FromCurrency,
                    ToCurrency = rate.Key,
                    ExchangRate = rate.Value,
                    AmountIn = request?.Amount,
                    AmountOut = amountConverted,
                    ExecutedDate = DateTime.UtcNow
                });
            }

            _dbContext?.SaveChanges();

            return new ExchangeCurrencyResponse
            {
                ClientId = request?.ClientId,
                CurrenciesConverted = convertedCurrencies,
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