using CurrencyExchange.Contracts;
using CurrencyExchange.Models.Requests;
using CurrencyExchange.Models.Responses;
using CurrencyExchange.Services;
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
        var response = _exchangeCurrencyService.ExchangeCurrency(request);

        if (!response.IsSuccess) return StatusCode(500, _exchangeCurrencyService.ExchangeCurrency(request));

        return Ok(response);
    }
}
