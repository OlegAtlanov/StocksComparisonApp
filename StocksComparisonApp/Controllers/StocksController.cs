using System;
using Microsoft.AspNetCore.Mvc;
using StocksComparisonApp.Infrastructure.Services.Stock;
using StocksComparisonApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Description;
using StocksComparisonApp.Infrastructure.DbContext;


namespace StocksComparisonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly StockContext _context;

        public StocksController(IStockService stockService, StockContext stockContext)
        {
            _stockService = stockService;
            _context = stockContext;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string stockSymbol)
        {
            if (!_context.Stocks.Any(s =>
                    string.Equals(s.Name, stockSymbol, StringComparison.CurrentCultureIgnoreCase)))
            {
                var result = await _stockService.GetStocksBySymbolAsync(stockSymbol);

                if (result == null)
                {
                    return NotFound();
                }

                await _context.Stocks.AddRangeAsync(result);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
