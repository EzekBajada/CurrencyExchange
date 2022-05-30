using Moq;
using NUnit.Framework;

namespace CurrencyExchange.UnitTests;

public class ExchangeCurrencyServiceUnitTests
{
    private Mock<IFixerIoService> _fixerIoServiceMock;
    private Mock<ILogger<ExchangeCurrencyService>> _loggerMock;
    private Mock<IRedisService> _redisServiceMock;
    private Mock<IRepository<Client>> _clientRepositoryMock;
    private Mock<IRepository<CurrencyExchangeHistory>> _currencyExchangeHistoryRepositoryMock;
    private Mock<AppSettings> _appSettingsMock;

    private ExchangeCurrencyService _exchangeCurrencyService;
    
    [SetUp]
    public void Setup()
    {
        _fixerIoServiceMock = new Mock<IFixerIoService>();
        _loggerMock = new Mock<ILogger<ExchangeCurrencyService>>();
        _redisServiceMock = new Mock<IRedisService>();
        _clientRepositoryMock = new Mock<IRepository<Client>>();
        _currencyExchangeHistoryRepositoryMock = new Mock<IRepository<CurrencyExchangeHistory>>();
        _appSettingsMock = new Mock<AppSettings>();

        _exchangeCurrencyService = new ExchangeCurrencyService(_fixerIoServiceMock.Object,
            _loggerMock.Object,
            _redisServiceMock.Object,
            _clientRepositoryMock.Object,
            _currencyExchangeHistoryRepositoryMock.Object, _appSettingsMock.Object);
        
        _redisServiceMock.Setup(x =>
                x.GetAsync<LatestExchangeRatesResponse>(It.IsAny<string>()))
            .Returns(Task.FromResult<LatestExchangeRatesResponse?>(default));
        
        _fixerIoServiceMock.Setup(x => x.GetCurrencyExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult<LatestExchangeRatesResponse?>(new LatestExchangeRatesResponse
            {
                TimeStamp = 1,
                Base = "EUR",
                Date = DateTime.Now,
                ErrorMessage = null,
                Rates = new Dictionary<string, double>()
                {
                    {
                        "USD", 1.3
                    }
                },
                Success = true
            }));

        _appSettingsMock.Setup(x => x.MaxAllowedRequests).Returns(10);
    }
    
    [Test]
    public async Task ExchangeCurrencyShouldReturnValue()
    {
        // Arrange
        var request = new ExchangeCurrencyRequest
        {
            ClientId = 1,
            Amount = 100,
            FromCurrency = "EUR",
            ToCurrency = "USD"
        };

        _clientRepositoryMock.Setup(x => x.GetOneByIdAsync(It.IsAny<int>()))
            .Returns(Task.FromResult(new Client{ClientId = 1, BaseCurrency = "EUR"})!);

        _currencyExchangeHistoryRepositoryMock?.Setup(x => 
                x.GetMultipleByFilterAsync(It.IsAny<Func<CurrencyExchangeHistory, bool>>()))
            .Returns(Task.FromResult<IEnumerable<CurrencyExchangeHistory>?>(new List<CurrencyExchangeHistory>
            {
                new()
                {
                    ClientId = 1
                }
            }));
        
        // Act
        var result = await _exchangeCurrencyService?.ExchangeCurrenciesAsync(request)!;
        
        
        // Assert
        var responseToAssert = new ExchangeCurrencyResponse
        {
            ClientId = 1,
            Success = true,
            CurrencyConverted = new ConvertedCurrency(100 * 1.3, "EUR", "USD", 1.3),
            ErrorMessage = null
        };

        Assert.AreEqual(responseToAssert.ClientId, result.ClientId);
        Assert.AreEqual(responseToAssert.CurrencyConverted, result.CurrencyConverted);
        Assert.AreEqual(responseToAssert.Success, result.Success);
        Assert.AreEqual(responseToAssert.ErrorMessage, result.ErrorMessage);
    }

    [Test]
    public async Task ExchangeCurrencyClientDoesntExist()
    {
        var request = new ExchangeCurrencyRequest
        {
            ClientId = 1222,
            Amount = 100,
            FromCurrency = "EUR",
            ToCurrency = "USD"
        };

        _clientRepositoryMock.Setup(x => x.GetOneByIdAsync(It.IsAny<int>()))
            .Returns(Task.FromResult<Client>(default!)!);

        _currencyExchangeHistoryRepositoryMock?.Setup(x => 
                x.GetMultipleByFilterAsync(It.IsAny<Func<CurrencyExchangeHistory, bool>>()))
            .Returns(Task.FromResult<IEnumerable<CurrencyExchangeHistory>?>(new List<CurrencyExchangeHistory>
            {
                new()
                {
                    ClientId = 1
                }
            }));
        
        var result = await _exchangeCurrencyService?.ExchangeCurrenciesAsync(request)!;
        
        Assert.AreEqual(null, result.ClientId);
        Assert.AreEqual(null, result.CurrencyConverted);
        Assert.AreEqual(false, result.Success);
        Assert.AreEqual("Client 1222 does not exist", result.ErrorMessage);
    }
    
    [Test]
    public async Task ExchangeCurrencyClientExceededCalls()
    {
        var request = new ExchangeCurrencyRequest
        {
            ClientId = 1,
            Amount = 100,
            FromCurrency = "EUR",
            ToCurrency = "USD"
        };

        _clientRepositoryMock.Setup(x => x.GetOneByIdAsync(It.IsAny<int>()))
            .Returns(Task.FromResult(new Client{ClientId = 1, BaseCurrency = "EUR", ClientName = "TestUser"})!);

        _currencyExchangeHistoryRepositoryMock.Setup(x => 
                x.GetMultipleByFilterAsync(It.IsAny<Func<CurrencyExchangeHistory, bool>>()))
            .Returns(Task.FromResult<IEnumerable<CurrencyExchangeHistory>?>(new List<CurrencyExchangeHistory>
            {
                new() {   ClientId = 1  }, new() {   ClientId = 1  }, new() {   ClientId = 1  }, new() {   ClientId = 1  }, new() {   ClientId = 1  }, new() {   ClientId = 1  },
                new() {   ClientId = 1  }, new() {   ClientId = 1  }, new() {   ClientId = 1  }, new() {   ClientId = 1  }, new() {   ClientId = 1  }, new() {   ClientId = 1  }
            }));
        
        var result = await _exchangeCurrencyService?.ExchangeCurrenciesAsync(request)!;
        
        Assert.AreEqual(null, result.ClientId);
        Assert.AreEqual(null, result.CurrencyConverted);
        Assert.AreEqual(false, result.Success);
        Assert.AreEqual("TestUser exceeded maximum exchanges for 1 hour", result.ErrorMessage);
    }
}