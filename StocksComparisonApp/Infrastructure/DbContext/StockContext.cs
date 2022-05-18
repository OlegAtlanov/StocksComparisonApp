using Microsoft.EntityFrameworkCore;
using StocksComparisonApp.Models;

namespace StocksComparisonApp.Infrastructure.DbContext
{
    public class StockContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public StockContext(DbContextOptions<StockContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
