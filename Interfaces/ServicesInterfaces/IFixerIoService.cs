namespace CurrencyExchange.Interfaces.ServicesInterfaces;

public interface IFixerIoService
{
    public Task<LatestExchangeRatesResponse?> GetCurrencyExchangeRate(string? baseCurrency, string? symbols);
}