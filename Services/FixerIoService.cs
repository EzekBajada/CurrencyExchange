using System.Web;
using System.Text.Json;

namespace CurrencyExchange.Services;

public class FixerIoService : IFixerIoService
{
    private readonly HttpClient _httpClient;
    private readonly FixerIoSettings _fixerIoSettings;
    private readonly ILogger<FixerIoService> _logger;

    public FixerIoService(HttpClient httpClient, FixerIoSettings fixerIoSettings, ILogger<FixerIoService> logger)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("apikey", fixerIoSettings.ApiKey);
        _fixerIoSettings = fixerIoSettings;
        _logger = logger;
    }

    public async Task<LatestExchangeRatesResponse?> GetCurrencyExchangeRate(string? baseCurrency, string? symbols)
    {
        try
        {
            if (baseCurrency == null || symbols == null)
            {
                var message = "Currency rate could be fetched: Request Empty";
                LoggingUtilities<FixerIoService>.LogInformation(message, _logger, true);

                return new LatestExchangeRatesResponse
                {
                    Success = false,
                    ErrorMessage = message
                };
            }

            var uri = BuildUri($"{_fixerIoSettings.BaseUrl}/latest", new Dictionary<string, string>
            {
                {"base", baseCurrency},
                {"symbols", symbols}
            });

            var response = await _httpClient.GetAsync(uri).Result.Content.ReadFromJsonAsync<LatestExchangeRatesResponse>();
            LoggingUtilities<FixerIoService>.LogInformation($"FixerIoResponse: {JsonSerializer.Serialize(response)} ", _logger);

            return response;
        }
        catch (Exception e)
        {
            LoggingUtilities<FixerIoService>.LogInformation(e.ToString(), _logger, true);

            return new LatestExchangeRatesResponse
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }
    }

    private static Uri BuildUri(string endPointUri, Dictionary<string, string>? parameters)
    {
        var uriBuilder = new UriBuilder(endPointUri);
        if (parameters == null || !parameters.Any()) return uriBuilder.Uri;
        
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var param in parameters)
        {
            query[param.Key] = param.Value;
        }
        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }
}