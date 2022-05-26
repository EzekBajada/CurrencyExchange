using System;
using System.Collections.Generic;

namespace CurrencyExchange.Models.Responses;

public class LatestExchangeRatesResponse
{
    public bool Success { get; set; }

    public long TimeStamp { get; set; }

    public string? Base { get; set; }

    public DateTime Date { get; set; }

    public Dictionary<string, double>? Rates { get; set; }
}