using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StocksComparisonApp.Infrastructure.Services.Stock
{
    public class StockService : IStockService
    {
        private readonly IMapper _autoMapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private const string TimeSeriesDailyFunction = "TIME_SERIES_DAILY";
        private const string SymbolSearch = "SYMBOL_SEARCH";

        public StockService(IHttpClientFactory httpClientFactory, IMapper autoMapper, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _autoMapper = autoMapper;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Models.Stock>> GetStocksBySymbolAsync(string stockSymbol)
        {
            using var httpClient = _httpClientFactory.CreateClient("AlphaVantageClient");

            // For checking correct symbols we could use function=SYMBOL_SEARCH, and after that call the main endpoint
            var result =
                await httpClient.GetAsync(
                    $"query?function={TimeSeriesDailyFunction}&symbol={stockSymbol}&apikey={_configuration["Security:ApiKey"]}");
            
            result.EnsureSuccessStatusCode();

            var json = await result.Content.ReadAsStringAsync();

            // Get value by index which specified in documentation
            // I see that this is not a good approach to use unreliable indexes like ["Time Series (Daily)"],
            // but using the last value which always should represents stocks (as described in docs) can be a part of error.
            var getResult = JObject.Parse(json)["Time Series (Daily)"];

            if (getResult != null)
            {
                // We take 7 rows for the current week
                var stocksForCurrentWeek = getResult.Children().Take(7);

                var stocks = new List<Models.Stock>();
                foreach (var stock in stocksForCurrentWeek)
                {
                    var stockFromRequest = stock.Children().First();

                    var mappedStock = _autoMapper.Map<Models.Stock>(stockFromRequest);
                    mappedStock.Name = stockSymbol;
                    stocks.Add(mappedStock);
                }

                return stocks;
            }

            return null;
        }

        public async Task<string> Search(string stockSymbol)
        {
            using var httpClient = _httpClientFactory.CreateClient("AlphaVantageClient");

            var result =
                await httpClient.GetAsync(
                    $"query?function={SymbolSearch}&keywords={stockSymbol}&apikey={_configuration["Security:ApiKey"]}");
            
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsStringAsync();
        }
    }
}
