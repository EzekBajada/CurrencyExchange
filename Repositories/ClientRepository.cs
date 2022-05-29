using System.Globalization;
using CurrencyExchange.Interfaces.RepositoryInterfaces;

namespace CurrencyExchange.Repositories;

public class ClientRepository : IRepository<Client>
{
    private readonly CurrencyExchangeDbContext _dbContext;
    private readonly ILogger<ClientRepository> _logger;

    public ClientRepository(CurrencyExchangeDbContext dbContext, ILogger<ClientRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public Task<Client?> GetOneById(int? id)
    {
        try
        {
            return Task.FromResult(_dbContext.Clients?.FirstOrDefault(x => x.ClientId == id));
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorMessages.RepositoryGetError);
            return Task.FromException<Client?>(e);
        }        
    }

    public Task<IEnumerable<Client>?> GetMultipleByFilter(Func<Client, bool> filter)
    {
        try
        {
            return Task.FromResult(_dbContext.Clients?.Where(filter));
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorMessages.RepositoryGetError);
            return Task.FromException<IEnumerable<Client>?>(e);
        }
    }

    public Task AddOne(Client entity)
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