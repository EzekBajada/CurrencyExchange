using CurrencyExchange.Models.Responses;

namespace CurrencyExchange.Contracts;

public interface IFixerIoService
{
    public Task<LatestExchangeRatesResponse?> GetCurrencyExchangeRate(string? baseCurrency, string? symbols);
}