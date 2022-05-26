using System;

namespace CurrencyExchange.Models;

public class CurrencyExchangeHistory
{
    public int CurrencyExchangeHistoryId { get; set; }

    public int ClientId { get; set; }

    public string? FromCurrency { get; set; }

    public string? ToCurrency { get; set; }

    public int AmountIn { get; set; }

    public int AmountOut { get; set; }

    public DateTime ExecutedDate { get; set; }
}