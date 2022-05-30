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

    public async Task<bool> PutAsync(string key, string value)
    {
        try
        {
            return await _database.StringSetAsync(key, value, TimeSpan.FromMinutes(30));
        }
        catch (Exception e)
        {            
            _logger.LogError(e, InfoErrorMessages.RedisPutError);
            return await Task.FromResult(false);
        }
    }

    public async Task<T?> GetAsync<T>(string? key)
    {
        try
        {
            var value = await _database.StringGetAsync(key);
            return !value.HasValue ? await Task.FromResult<T?>(default) : await Task.FromResult(JsonSerializer.Deserialize<T>(value));
        }
        catch (Exception e)
        {
            _logger.LogError(e, InfoErrorMessages.RedisGetError); 
            return await Task.FromException<T?>(e);
        }
    }
}