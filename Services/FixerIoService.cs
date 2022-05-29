using System.Collections.Specialized;
using System.Web;

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
                _logger.LogError(InfoErrorMessages.FixerIoEmptyRequest);
                return new LatestExchangeRatesResponse
                {
                    Success = false,
                    ErrorMessage = InfoErrorMessages.FixerIoEmptyRequest
                };
            }

            var uri = await BuildUri($"{_fixerIoSettings.BaseUrl}/latest", new Dictionary<string, string>
            {
                {"base", baseCurrency},
                {"symbols", symbols}
            });

            var httpResponseMessage = await _httpClient.GetAsync(uri);
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<LatestExchangeRatesResponse>();
                
            _logger.LogInformation(InfoErrorMessages.FixerIoResponseLog, uri.ToString(), JsonSerializer.Serialize(response));

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(InfoErrorMessages.FixerIoFetchError);
            return new LatestExchangeRatesResponse
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }
    }

    private static async Task<Uri> BuildUri(string endPointUri, Dictionary<string, string>? parameters)
    {
        var uriBuilder = new UriBuilder(endPointUri);
        if (parameters == null || !parameters.Any()) return uriBuilder.Uri;
        
        var query = await new Task<NameValueCollection>(() => HttpUtility.ParseQueryString(uriBuilder.Query));
        foreach (var param in parameters)
        {
            query[param.Key] = param.Value;
        }
        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }
}