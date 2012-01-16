using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.DataAccess
{
    public interface IStringStockDataProvider
    {
        string GetLatestRawStockData(string stockSymbol);

    }
}
