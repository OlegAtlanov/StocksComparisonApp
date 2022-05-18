using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StocksComparisonApp.Infrastructure.Services.Comparison;
using StocksComparisonApp.Infrastructure.Services.Stock;
using StocksComparisonApp.Middleware;
using System;
using Microsoft.EntityFrameworkCore;
using StocksComparisonApp.Infrastructure.DbContext;

namespace StocksComparisonApp
{
    public class Startup
    {
        private const string CommonStock = "SPY";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpClient("AlphaVantageClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://www.alphavantage.co/");
            });
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IComparisonStocksService, ComparisonStocksService>();
            services.AddSwaggerGen();
            services.AddDbContext<StockContext>(opt => opt.UseInMemoryDatabase("StockDB"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Added swagger to more suitable view to check all endpoints
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            // Add pre-defined data
            AddPreDefinedData(app.ApplicationServices);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddPreDefinedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetService<StockContext>();
            var stockService = scope.ServiceProvider.GetService<IStockService>();

            var result = stockService?.GetStocksBySymbolAsync(CommonStock).Result;

            if (result != null)
            {
                if (context != null)
                {
                    context.Stocks.AddRange(result);
                    context.SaveChanges();
                }
            }
        }
    }
}
