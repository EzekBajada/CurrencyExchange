namespace CurrencyExchange.Interfaces.ServicesInterfaces;

public interface IExchangeCurrencyService
{
    public Task<ExchangeCurrencyResponse> ExchangeCurrencies(ExchangeCurrencyRequest request);
}