using StocksComparisonApp.Models.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StocksComparisonApp.Infrastructure.Services.Comparison
{
    public interface IComparisonStocksService
    {
        Task<IEnumerable<ComparisonResult>> GetComparisonWithSpyStockAsync(string stockSymbol);
    }
}
