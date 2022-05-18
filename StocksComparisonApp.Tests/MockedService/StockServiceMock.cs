using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RichardSzalay.MockHttp;
using StocksComparisonApp.App_Start.AutoMapperConfig;
using StocksComparisonApp.Infrastructure.DbContext;
using StocksComparisonApp.Infrastructure.Services.Stock;
using System;
using System.IO;
using System.Net.Http;

namespace StocksComparisonApp.Tests.MockedService
{
    public static class StockServiceMock
    {
        public static IStockService Create(string funcName)
        {
            var config = new ConfigurationBuilder().AddJsonFile("MockedData/appsettings_test.json").Build();

            var mockHttp = new MockHttpMessageHandler();

            string validResponse;

            using (StreamReader r = new StreamReader("MockedData/valid_resopnse.json"))
            {
                validResponse = r.ReadToEnd();
            }

            mockHttp.When($"https://www.alphavantage.co/query?function={funcName}&symbol=stock_not_found&apikey={config["Security:ApiKey"]}")
                .Respond("application/json", "{}");

            mockHttp.When($"https://www.alphavantage.co/query?function={funcName}&symbol=IBM&apikey={config["Security:ApiKey"]}")
                .Respond("application/json", validResponse);

            mockHttp.When($"https://www.alphavantage.co/query?function={funcName}&symbol=SPY&apikey={config["Security:ApiKey"]}")
                .Respond("application/json", validResponse);

            var client = new HttpClient(mockHttp);
            client.BaseAddress = new Uri("https://www.alphavantage.co/");

            var mockFactory = new Mock<IHttpClientFactory>();

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var autoMapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            
            //create In Memory Database
            var options = new DbContextOptionsBuilder<StockContext>()
                .UseInMemoryDatabase(databaseName: "StockDB")
                .Options;

            // Also we can mock it like this.
            // But using InMemory does not allow to mock context without explicit call of UseInMemoryDatabase

            //var mockSet = new Mock<DbSet<Stock>>();
            //var mockContext = new Mock<StockContext>();
            //mockContext.Setup(m => m.Stocks).Returns(mockSet.Object);

            return new StockService(mockFactory.Object, autoMapper.CreateMapper(), config, new StockContext(options));
        }
    }
}
