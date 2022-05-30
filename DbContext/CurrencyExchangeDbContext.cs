namespace CurrencyExchange.DbContext;

public class CurrencyExchangeDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<CurrencyExchangeHistory> CurrencyExchangeHistories => Set<CurrencyExchangeHistory>();

    public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options)
        : base(options)
    { }
}