using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeCurrencyController : ControllerBase
{
    private readonly IExchangeCurrencyService _exchangeCurrencyService;

    public ExchangeCurrencyController(IExchangeCurrencyService exchangeCurrencyService)
    {
        _exchangeCurrencyService = exchangeCurrencyService;
    }

    [HttpPost]
    public IActionResult ExchangeCurrency(ExchangeCurrencyRequest request)
    {
        var response = _exchangeCurrencyService.ExchangeCurrencies(request);

        return !response.Success ? StatusCode(500, response) : Ok(response);
    }
}
