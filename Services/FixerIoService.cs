using System.Web;
using CurrencyExchange.Models;
using CurrencyExchange.Models.Responses;

namespace CurrencyExchange.Services;

public class FixerIoService
{
    private readonly HttpClient _httpClient;
    private readonly FixerIoSettings _fixerIoSettings;

    public FixerIoService(HttpClient httpClient, FixerIoSettings fixerIoSettings)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("apikey", fixerIoSettings.ApiKey);
        _fixerIoSettings = fixerIoSettings;
    }

    public async Task<LatestExchangeRatesResponse?> GetCurrencyExchangeRate(string baseCurrency, string symbols)
    {
        try
        {
            var response = await _httpClient.GetAsync(BuildUri($"{_fixerIoSettings.BaseUrl}/latest", new Dictionary<string, string>
            {
                {"base", baseCurrency},
                {"symbols", symbols}
            }));

            return response.Content.ReadFromJsonAsync<LatestExchangeRatesResponse>().Result;

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    private static Uri BuildUri(string endPointUri, Dictionary<string, string>? parameters)
    {
        var uriBuilder = new UriBuilder(endPointUri);

        if (parameters != null && parameters.Any())
        {
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var param in parameters)
            {
                query[param.Key] = param.Value;
            }
            uriBuilder.Query = query.ToString();
        }
        return uriBuilder.Uri;
    }
}