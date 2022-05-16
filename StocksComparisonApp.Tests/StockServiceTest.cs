using StocksComparisonApp.Infrastructure.Services.Stock;
using StocksComparisonApp.Tests.MockedService;
using System.Linq;
using Xunit;

namespace StocksComparisonApp.Tests
{
    public class StockServiceTest
    {
        private readonly IStockService _stockService;

        private const string TimeSeriesDailyFunction = "TIME_SERIES_DAILY";

        public StockServiceTest()
        {
            _stockService = StockServiceMock.Create(TimeSeriesDailyFunction);
        }

        [Fact]
        public void Given_Not_Valid_Stock()
        {
            var result = _stockService.GetStocksBySymbolAsync("stock_not_found").Result;

            Assert.Null(result);
        }

        [Fact]
        public void Given_Valid_Stock()
        {
            var result = _stockService.GetStocksBySymbolAsync("IBM").Result;

            Assert.NotNull(result);
            Assert.Equal(7, result.Count());

            var currentDay = result.FirstOrDefault();

            Assert.NotNull(currentDay);

            Assert.Equal("IBM", currentDay.Name);
            Assert.Equal("5/13/2022", currentDay.Date.ToShortDateString());
            Assert.Equal(133, currentDay.Open);
            Assert.Equal(133.8M, currentDay.High);
            Assert.Equal(131.05M, currentDay.Low);
            Assert.Equal(133.6M, currentDay.Close);
            Assert.Equal(4195218, currentDay.Volume);
        }
    }
}
