using CurrencyExchange.Models.FixerIo;
using CurrencyExchange.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables(prefix: "CurrencyExchange_")
    .Build();

// Add services to the container.
builder.Services.Configure<FixerIoSettings>(config.GetSection(nameof(FixerIoSettings)));
builder.Services.AddSingleton<FixerIoSettings>(x => x.GetRequiredService<IOptions<FixerIoSettings>>().Value);

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
