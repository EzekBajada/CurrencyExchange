namespace CurrencyExchange.Models;

public class CurrencyExchangeHistory
{
    public int CurrencyExchangeHistoryId { get; set; }

    public int? ClientId { get; set; }

    public virtual Client? Client { get; set; }

    public string? FromCurrency { get; set; }

    public string? ToCurrency { get; set; }

    public double ExchangeRate { get; set; }

    public double? AmountIn { get; set; }

    public double? AmountOut { get; set; }

    public DateTime ExecutedDate { get; set; }
}