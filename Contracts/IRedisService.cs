using StackExchange.Redis;

namespace CurrencyExchange.Contracts;

public interface IRedisService
{
    public void Put(string key, RedisValue value);

    public RedisValue Get(string key);
}