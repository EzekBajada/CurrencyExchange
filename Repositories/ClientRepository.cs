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

    public async Task<Client?> GetOneByIdAsync(int? id)
    {
        return await _dbContext.Clients?.FirstOrDefaultAsync(x => x.ClientId == id)!;
    }

    public async Task<IEnumerable<Client>?> GetMultipleByFilterAsync(Func<Client, bool> filter)
    {
        return await Task.FromResult(_dbContext.Clients?.Where(filter));
    }

    public async Task AddOneAsync(Client entity)
    {
        await _dbContext.AddAsync(entity) ;
    }

    public async Task SaveDbChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}