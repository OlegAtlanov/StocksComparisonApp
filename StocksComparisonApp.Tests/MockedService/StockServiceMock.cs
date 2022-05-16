using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using RichardSzalay.MockHttp;
using StocksComparisonApp.App_Start.AutoMapperConfig;
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

            return new StockService(mockFactory.Object, autoMapper.CreateMapper(), config);
        }
    }
}
