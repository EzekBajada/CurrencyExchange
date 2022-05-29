namespace CurrencyExchange.Utilities;

public static class ErrorMessages
{
    public static string ClientNotFound(int? clientId)
    {
        return $"Client {clientId} does not exist";
    }

    public static string ExceededAllowedRequests(string? clientName)
    {
     return $"{clientName} exceeded maximum exchanges for 1 hour";
        
    } 

    public const string CurrencyRateCacheError = "Currency rate could not be cached";

    public const string FixerIoFetchError = "Currency rate could not be fetched: FixerIo error";

    public const string FixerIoEmptyRequest = "Currency rate could be fetched: Request Empty";
    
    public const string FixerIoResponseLog = "FixerIo Request: {request} and response: {response}";
    
    public const string RedisPutError = "Redis error when setting";

    public const string RedisGetError = "Redis error when fetching";

    public const string RepositoryGetError = "Error when getting data from database";
    
    public const string RepositoryAddError = "Error when inserting data in database";

    public const string RepositorySaveError = "Error when saving changes in database";

    public const string ExchangeRateGeneralError = "Error when attempting to calculcate exchange rate";
}