using Microsoft.AspNetCore.Mvc;
using StocksComparisonApp.Infrastructure.Services.Comparison;
using System.Threading.Tasks;

namespace StocksComparisonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksComparisonController : ControllerBase
    {
        private readonly IComparisonStocksService _aggregatorStockService;

        public StocksComparisonController(IComparisonStocksService aggregatorStockService)
        {
            _aggregatorStockService = aggregatorStockService;
        }

        [HttpGet("{stockSymbol}")]
        public async Task<IActionResult> Get(string stockSymbol)
        {
            var result = await _aggregatorStockService.GetComparisonWithSpyStockAsync(stockSymbol);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
