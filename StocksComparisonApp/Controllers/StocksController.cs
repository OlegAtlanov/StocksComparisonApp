using Microsoft.AspNetCore.Mvc;
using StocksComparisonApp.Infrastructure.Services.Stock;
using StocksComparisonApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Description;


namespace StocksComparisonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StocksController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("search/{stockSymbol}")]
        [ResponseType(typeof(string))]
        public async Task<IActionResult> Search(string stockSymbol)
        {
            var result = await _stockService.Search(stockSymbol);

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{stockSymbol}")]
        [ResponseType(typeof(IEnumerable<Stock>))]
        public async Task<IActionResult> Get(string stockSymbol)
        {
            var result = await _stockService.GetStocksBySymbolAsync(stockSymbol);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
