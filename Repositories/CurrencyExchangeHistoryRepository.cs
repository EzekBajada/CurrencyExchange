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

    public Task<CurrencyExchangeHistory?> GetOneById(int? id)
    {
        try
        {
            return Task.FromResult(_dbContext.CurrencyExchangeHistories?.FirstOrDefault(x => x.CurrencyExchangeHistoryId == id));
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorMessages.RepositoryGetError);
            return Task.FromException<CurrencyExchangeHistory?>(e);
        }
    }

    public Task<IEnumerable<CurrencyExchangeHistory>?> GetMultipleByFilter(Func<CurrencyExchangeHistory, bool> filter)
    {
        try
        {
            return Task.FromResult(_dbContext.CurrencyExchangeHistories?.Where(filter));
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorMessages.RepositoryGetError);
            return Task.FromException<IEnumerable<CurrencyExchangeHistory>?>(e);
        }
    }

    public Task AddOne(CurrencyExchangeHistory entity)
    {
        try
        {
            return Task.FromResult(_dbContext.Add(entity)) ;
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorMessages.RepositoryAddError);
            return Task.FromException(e);
        }
    }

    public Task SaveDbChanges()
    {
        try
        {
            return Task.FromResult(_dbContext.SaveChanges());
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorMessages.RepositorySaveError);
            return Task.FromException(e);
        }
    }
}