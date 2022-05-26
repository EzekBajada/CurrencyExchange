using CurrencyExchange.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.DbContext;

public class CurrencyExchangeDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Client>? Clients { get; set; }
    public DbSet<CurrencyExchangeHistory>? CurrencyExchangeHistories { get; set; }

    public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options)
        : base(options)
    { }
}