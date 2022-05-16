using StocksComparisonApp.Infrastructure.Services.Comparison;
using StocksComparisonApp.Tests.MockedService;
using Xunit;

namespace StocksComparisonApp.Tests
{
    public class ComparisonStocksServiceTest
    {
        private readonly IComparisonStocksService _comparisonStocksService;

        private const string TimeSeriesDailyFunction = "TIME_SERIES_DAILY";

        public ComparisonStocksServiceTest()
        {
            _comparisonStocksService = new ComparisonStocksService(StockServiceMock.Create(TimeSeriesDailyFunction));
        }

        [Fact]
        public void Comparison_Valid_Stocks()
        {
            var result = _comparisonStocksService.GetComparisonWithSpyStockAsync("IBM").Result;

            Assert.NotNull(result);
        }
    }
}
