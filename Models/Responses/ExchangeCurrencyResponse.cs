namespace CurrencyExchange.Models.Responses;

public record ConvertedCurrency(double? AmountConverted, string? FromCurrency, string? ToCurrency, double? ExchangeRate);

public class ExchangeCurrencyResponse : BaseResponse
{
    public int? ClientId { get; set; }

    public ConvertedCurrency? CurrencyConverted { get; set; }
}