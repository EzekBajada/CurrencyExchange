namespace CurrencyExchange.Contracts;

public interface IExchangeCurrencyService
{
    public ExchangeCurrencyResponse ExchangeCurrencies(ExchangeCurrencyRequest request);
}