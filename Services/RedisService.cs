using StackExchange.Redis;

namespace CurrencyExchange.Services;

public class RedisService : IRedisService
{
    private IConnectionMultiplexer _redis;
    private IDatabase _database;

    public RedisService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public RedisValue Get(string key)
    {
        return _database.StringGet(key);
    }

    public void Put(string key, RedisValue value)
    {
        _database.StringSet(key, value);
    }
}