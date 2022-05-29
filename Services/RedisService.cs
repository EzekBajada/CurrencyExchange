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

    public Task<bool> Put(string key, string value)
    {
        try
        {
            return Task.FromResult(_database.StringSet(key, value));
        }
        catch (Exception e)
        {            
            _logger.LogError(e, ErrorMessages.RedisPutError);
            return Task.FromResult(false);
        }
    }

    public Task<T?> Get<T>(string? key)
    {
        try
        {
            var value = _database.StringGet(key);
            return !value.HasValue ? Task.FromResult<T?>(default) : Task.FromResult(JsonSerializer.Deserialize<T>(value));
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorMessages.RedisGetError); 
            return Task.FromException<T?>(e);
        }
    }
}