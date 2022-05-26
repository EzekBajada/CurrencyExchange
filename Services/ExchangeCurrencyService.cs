using CurrencyExchange.Contracts;
using CurrencyExchange.DbContext;
using CurrencyExchange.Models.Requests;
using CurrencyExchange.Models.Responses;

namespace CurrencyExchange.Services;

public class ExchangeCurrencyService : IExchangeCurrencyService
{
    private readonly CurrencyExchangeDbContext _dbContext;
    private readonly IFixerIoService _fixerIoService;

    public ExchangeCurrencyService(CurrencyExchangeDbContext dbContext, IFixerIoService fixerIoService)
    {
        _dbContext = dbContext;
        _fixerIoService = fixerIoService;
    }

    public ExchangeCurrencyResponse ExchangeCurrency(ExchangeCurrencyRequest request)
    {
        try
        {
            var currencyRate = _fixerIoService.GetCurrencyExchangeRate(request?.FromCurrency, request?.ToCurrency).Result;

            if (currencyRate != null && !currencyRate.Success)
            {
                return new ExchangeCurrencyResponse
                {
                    IsSuccess = false
                };
            }

            var rate = currencyRate.Rates[request.ToCurrency];


            return new ExchangeCurrencyResponse
            {
                ClientId = request.ClientId,
                AmountConverted = request.Amount * rate,
                IsSuccess = true
            };
        }
        catch (Exception e)
        {
            return new ExchangeCurrencyResponse
            {
                IsSuccess = false
            };
        }
    }
}