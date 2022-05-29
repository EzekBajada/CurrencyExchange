using System.Diagnostics.CodeAnalysis;
using CurrencyExchange.Interfaces.RepositoryInterfaces;

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

    public async Task<CurrencyExchangeHistory?> GetOneById(int? id)
    {
        try
        {
            return await _dbContext.CurrencyExchangeHistories?.FirstOrDefaultAsync(x => x.CurrencyExchangeHistoryId == id)!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, InfoErrorMessages.RepositoryGetError);
            return await Task.FromException<CurrencyExchangeHistory?>(e);
        }
    }

    public async Task<IEnumerable<CurrencyExchangeHistory>?> GetMultipleByFilter(Func<CurrencyExchangeHistory, bool> filter)
    {
        try
        {
            return await Task.FromResult(_dbContext.CurrencyExchangeHistories?.Where(filter));
        }
        catch (Exception e)
        {
            _logger.LogError(e, InfoErrorMessages.RepositoryGetError);
            return await Task.FromException<IEnumerable<CurrencyExchangeHistory>?>(e);
        }
    }

    public async Task AddOne(CurrencyExchangeHistory entity)
    {
        try
        {
            await _dbContext.AddAsync(entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, InfoErrorMessages.RepositoryAddError);
            await Task.FromException(e);
        }
    }

    public async Task SaveDbChanges()
    {
        try
        {
           await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, InfoErrorMessages.RepositorySaveError);
            await Task.FromException(e);
        }
    }
}