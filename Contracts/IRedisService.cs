namespace CurrencyExchange.Contracts;

public interface IRedisService
{
    public bool Put(string key, string value);

    public T? Get<T>(string? key);
}