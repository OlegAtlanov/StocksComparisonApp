# StocksComparisonApp

This is an application which shows us a difference between prices of the given stock and compare it with SPY stock.

Just run an application and use swagger for trying out all endpoints.

I created 3 endpoints:
  1. Serach endpont which helps to find proper stock symbol.
  2. Get endpoint by stock symbol.
  3. Get endpoint for comparison.

For stocks source I used https://www.alphavantage.co/documentation/.
Before running the application, please call in browser endpoint https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&apikey=demo, to check the latest refreshed day.

I generated my own key, so, you can use it, or generate your own and put it into section "Security" in appsettings.json.
