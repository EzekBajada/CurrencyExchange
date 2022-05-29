namespace CurrencyExchange.Interfaces.ServicesInterfaces;

public interface IExchangeCurrencyService
{
    public Task<ExchangeCurrencyResponse> ExchangeCurrenciesAsync(ExchangeCurrencyRequest request);
}