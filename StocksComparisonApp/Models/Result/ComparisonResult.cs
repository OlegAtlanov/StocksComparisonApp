using System;

namespace StocksComparisonApp.Models.Result
{
    // For test application I created strong naming fields for result.
    // In real life application it would be something generic to us it everywhere.
    public class ComparisonResult
    {
        public DateTime Date { get; set; }

        public string GivenStock { get; set; }

        public string SpyStock { get; set; }
    }
}
