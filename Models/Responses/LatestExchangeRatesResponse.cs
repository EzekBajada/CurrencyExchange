namespace CurrencyExchange.Models.Responses;

public class LatestExchangeRatesResponse
{
    public bool Success { get; set; }

    public string? TimeStamp { get; set; }

    public string? Base { get; set; }

    public DateOnly Date { get; set; }

    public Dictionary<string, int>? Rates { get; set; }
}