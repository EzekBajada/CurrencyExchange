namespace CurrencyExchange.Models.Configurations;

public class FixerIoSettings
{
    public virtual string? ApiKey { get; init; }

    public virtual string? BaseUrl { get; init; }
}