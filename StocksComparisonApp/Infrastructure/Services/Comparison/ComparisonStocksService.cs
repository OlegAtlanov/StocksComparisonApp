using StocksComparisonApp.Infrastructure.DbContext;
using StocksComparisonApp.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StocksComparisonApp.Infrastructure.Services.Comparison
{
    public class ComparisonStocksService : IComparisonStocksService
    {
        private readonly StockContext _context;

        private const string CommonStock = "SPY";
        public ComparisonStocksService(StockContext stockContext)
        {
            _context = stockContext;
        }

        public async Task<IEnumerable<ComparisonResult>> GetComparisonWithSpyStockAsync(string stockSymbol)
        {
            var requestedStocks = await _context.Stocks
                .Where(s => string.Equals(s.Name, stockSymbol, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();

            if (!requestedStocks.Any())
            {
                // Throwing an error like an example to show how error handler works.
                // In real life application, I would create custom exception and then in error handler handle it as an 4** error.
                throw new Exception(
                    $"There is no such data for {stockSymbol} in DB. Please call Post method to store {stockSymbol} stock.");
            }

            var spyStocks = await _context.Stocks
                .Where(s => string.Equals(s.Name, CommonStock, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();

            if (!spyStocks.Any())
            {
                throw new Exception(
                    $"There is no such data for {CommonStock} in DB. Please call Post method to store {CommonStock} stock.");
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
                        $"{PerformComparison(priceAtTheBeginningOfTheWeekSpy, spyStocks.FirstOrDefault(s => s.Date == requestedStock.Date)!.High):0.0#}%")
                });
            }

            return list;
        }

        private decimal PerformComparison(decimal initialPrice, decimal currentPrice) => currentPrice / initialPrice * 100 - 100;
    }
}
