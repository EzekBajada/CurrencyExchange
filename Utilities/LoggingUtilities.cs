namespace CurrencyExchange.Utilities;

public static class LoggingUtilities<T>
{
    public static void LogInformation(string message, ILogger<T> logger, bool isError = false)
    {
        if (isError) logger.LogError(message);

        else logger.LogInformation(message);
    }
}