namespace CurrencyExchange.Repositories;

public class CurrencyExchangeHistoryRepository : IRepository<CurrencyExchangeHistory>
{
    private readonly CurrencyExchangeDbContext _dbContext;
    private readonly ILogger<CurrencyExchangeHistoryRepository> _logger;

    public CurrencyExchangeHistoryRepository(CurrencyExchangeDbContext dbContext, ILogger<CurrencyExchangeHistoryRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<CurrencyExchangeHistory?> GetOneByIdAsync(int? id)
    {
        return await _dbContext.CurrencyExchangeHistories?.FirstOrDefaultAsync(x => x.CurrencyExchangeHistoryId == id)!;
    }

    public async Task<IEnumerable<CurrencyExchangeHistory>?> GetMultipleByFilterAsync(Func<CurrencyExchangeHistory, bool> filter)
    {
        return await Task.FromResult(_dbContext.CurrencyExchangeHistories?.Where(filter));
    }

    public async Task AddOneAsync(CurrencyExchangeHistory entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public async Task SaveDbChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}