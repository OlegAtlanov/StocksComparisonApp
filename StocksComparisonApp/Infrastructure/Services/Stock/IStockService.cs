using System.Collections.Generic;
using System.Threading.Tasks;

namespace StocksComparisonApp.Infrastructure.Services.Stock
{
    public interface IStockService
    {
        Task<IEnumerable<Models.Stock>> GetStocksBySymbolAsync(string stockSymbol);
        Task<string> Search(string stockSymbol);
    }
}
