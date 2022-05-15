using StocksComparisonApp.Infrastructure.Services.Stock;
using StocksComparisonApp.Models.Result;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksComparisonApp.Infrastructure.Services.Comparison
{
    public class ComparisonStocksService : IComparisonStocksService
    {
        private readonly IStockService _stockService;

        private const string CommonStock = "SPY";
        public ComparisonStocksService(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<IEnumerable<ComparisonResult>> GetComparisonWithSpyStockAsync(string stockSymbol)
        {
            var requestedStocks = await _stockService.GetStocksBySymbolAsync(stockSymbol);

            if (requestedStocks == null)
            {
                return null;
            }

            var spyStocks = await _stockService.GetStocksBySymbolAsync(CommonStock);

            if (spyStocks == null)
            {
                return null;
            }

            var priceAtTheBeginningOfTheWeek = requestedStocks.Last().High;
            var priceAtTheBeginningOfTheWeekSpy = spyStocks.Last().High;

            var list = new List<ComparisonResult>();

            foreach (var requestedStock in requestedStocks)
            {
                list.Add(new ComparisonResult()
                {
                    Date = requestedStock.Date,
                    GivenStock =
                        string.Format($"{PerformComparison(priceAtTheBeginningOfTheWeek, requestedStock.High):0.0#}%"),
                    SpyStock = string.Format(
                        $"{PerformComparison(priceAtTheBeginningOfTheWeekSpy, spyStocks.FirstOrDefault(s => s.Date == requestedStock.Date).High):0.0#}%")
                });
            }

            return list;
        }

        private decimal PerformComparison(decimal initialPrice, decimal currentPrice) => ((currentPrice / initialPrice) * 100) - 100;
    }
}
