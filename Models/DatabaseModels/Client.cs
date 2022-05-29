namespace CurrencyExchange.Models.DatabaseModels;

public class Client
{
    public int ClientId { get; set; }

    public string? ClientName { get; set; }

    public string? BaseCurrency { get; set; }
}