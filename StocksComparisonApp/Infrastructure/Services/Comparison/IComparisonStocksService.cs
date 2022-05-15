using System.Collections.Generic;
using System.Threading.Tasks;
using StocksComparisonApp.Models.Result;

namespace StocksComparisonApp.Infrastructure.Services.Comparison
{
    public interface IComparisonStocksService
    {
        Task<IEnumerable<ComparisonResult>> GetComparisonWithSpyStockAsync(string stockSymbol);
    }
}
