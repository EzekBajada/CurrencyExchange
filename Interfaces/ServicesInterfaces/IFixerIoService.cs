namespace CurrencyExchange.Interfaces.ServicesInterfaces;

public interface IFixerIoService
{
    public Task<LatestExchangeRatesResponse?> GetCurrencyExchangeRateAsync(string? baseCurrency, string? symbols);
}