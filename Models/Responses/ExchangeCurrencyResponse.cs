namespace CurrencyExchange.Models.Responses;

public class ExchangeCurrencyResponse
{
    public int ClientId { get; set; }

    public double AmountConverted { get; set; }

    public bool IsSuccess { get; set; }
}