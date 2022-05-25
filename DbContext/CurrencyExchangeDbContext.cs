using CurrencyExchange.Models;
using Microsoft.EntityFrameworkCore;

public class CurrencyExchangeDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<CurrencyExchangeHistory> CurrencyExchangeHistories { get; set; }

    public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options)
        : base(options)
    { }
}