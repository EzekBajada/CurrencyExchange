global using System;
global using CurrencyExchange.Contracts;
global using CurrencyExchange.DbContext;
global using CurrencyExchange.Models;
global using CurrencyExchange.Models.FixerIo;
global using CurrencyExchange.Models.Requests;
global using CurrencyExchange.Models.Responses;
global using CurrencyExchange.Services;
global using CurrencyExchange.Utilities;
global using Microsoft.EntityFrameworkCore;
global using System.Text.Json;
global using StackExchange.Redis;

using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables(prefix: "CurrencyExchange_")
    .Build();

// Add services to the container.
builder.Services.Configure<FixerIoSettings>(config.GetSection(nameof(FixerIoSettings)));
builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<FixerIoSettings>>().Value);
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(config.GetSection("RedisConnectionString").Value));
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddScoped<IFixerIoService, FixerIoService>();
builder.Services.AddScoped<IExchangeCurrencyService, ExchangeCurrencyService>();

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
