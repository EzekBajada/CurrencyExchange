namespace CurrencyExchange.Interfaces.ServicesInterfaces;

public interface IRedisService
{
    public Task<bool> Put(string key, string value);

    public Task<T?>? Get<T>(string? key);
}