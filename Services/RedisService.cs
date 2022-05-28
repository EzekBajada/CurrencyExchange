

namespace CurrencyExchange.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _database;
    private readonly ILogger<RedisService> _logger;

    public RedisService(IConnectionMultiplexer redis, ILogger<RedisService> logger)
    {
        _database = redis.GetDatabase();
        _logger = logger;
    }

    public bool Put(string key, string value)
    {
        try
        {
            return _database.StringSet(key, value);
        }
        catch (Exception e)
        {            
            LoggingUtilities<RedisService>.LogInformation(e.ToString(), _logger, true);
            return false;
        }
    }

    public T? Get<T>(string? key)
    {
        try
        {
            var value = _database.StringGet(key);
            return !value.HasValue ? default : JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception e)
        {
            LoggingUtilities<RedisService>.LogInformation(e.ToString(), _logger, true);
            return default;
        }
    }
}