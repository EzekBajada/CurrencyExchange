namespace CurrencyExchange.Models.Responses;

public class BaseResponse
{
    public bool Success { get; set; }

    public string? ErrorMessage { get; set; }
}