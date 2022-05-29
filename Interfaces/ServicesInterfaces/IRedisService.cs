namespace CurrencyExchange.Interfaces.ServicesInterfaces;

public interface IRedisService
{
    public Task<bool> PutAsync(string key, string value);

    public Task<T?>? GetAsync<T>(string? key);
}