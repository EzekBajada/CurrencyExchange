namespace CurrencyExchange.Models.Requests;

public class ExchangeCurrencyRequest
{
    public int? ClientId { get; set; }

    public double Amount { get; set; }

    public string? FromCurrency { get; set; }

    public string? ToCurrency { get; set; }
}