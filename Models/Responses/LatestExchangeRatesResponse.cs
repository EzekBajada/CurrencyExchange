namespace CurrencyExchange.Models.Responses;

public class LatestExchangeRatesResponse : BaseResponse
{
    public long TimeStamp { get; set; }

    public string? Base { get; set; }

    public DateTime Date { get; set; }

    public Dictionary<string, double>? Rates { get; set; }
}