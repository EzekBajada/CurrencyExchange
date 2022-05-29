global using System;
global using CurrencyExchange.DbContext;
global using CurrencyExchange.Models.DatabaseModels;
global using CurrencyExchange.Models.Requests;
global using CurrencyExchange.Models.Responses;
global using CurrencyExchange.Services;
global using CurrencyExchange.Utilities;
global using CurrencyExchange.Interfaces.ServicesInterfaces;
global using Microsoft.EntityFrameworkCore;
global using System.Text.Json;
global using StackExchange.Redis;
global using CurrencyExchange.Interfaces.RepositoryInterfaces;
global using CurrencyExchange.Models.Configurations;

using CurrencyExchange.Repositories;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables(prefix: "CurrencyExchange_")
    .Build();

// Add services to the container.
builder.Services.Configure<FixerIoSettings>(config.GetSection(nameof(FixerIoSettings)));
builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<FixerIoSettings>>().Value);
builder.Services.Configure<AppSettings>(config.GetSection(nameof(AppSettings)));
builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<AppSettings>>().Value);

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(config.GetSection("RedisConnectionString").Value));
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddScoped<IFixerIoService, FixerIoService>();
builder.Services.AddScoped<IExchangeCurrencyService, ExchangeCurrencyService>();
builder.Services.AddScoped<IRepository<Client>, ClientRepository>();
builder.Services.AddScoped<IRepository<CurrencyExchangeHistory>, CurrencyExchangeHistoryRepository>();

builder.Services.AddLogging(loggingBuilder =>
  {
      var loggingSection = config.GetSection("Logging");
      loggingBuilder.AddFile(loggingSection);
  });

builder.Services.AddDbContext<CurrencyExchangeDbContext>(options =>
       options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<FixerIoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
