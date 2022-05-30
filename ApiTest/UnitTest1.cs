using Moq;
using NUnit.Framework;

namespace CurrencyExchange.ApiTest;

public class Tests
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
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}