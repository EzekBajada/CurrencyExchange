using CurrencyExchange.Models.Requests;
using CurrencyExchange.Models.Responses;

namespace CurrencyExchange.Contracts;

public interface IExchangeCurrencyService
{
    public ExchangeCurrencyResponse ExchangeCurrency(ExchangeCurrencyRequest request);
}