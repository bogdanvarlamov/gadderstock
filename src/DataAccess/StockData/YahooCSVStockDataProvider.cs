using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace _5DSolutions.Stocks.DataAccess
{
    public class YahooCSVStockDataProvider: IStringStockDataProvider
    {

        WebStockDataProvider _webProvider;


        public YahooCSVStockDataProvider()
        {
            _webProvider = new WebStockDataProvider();
        }


        #region IStockDataProvider Members

        public string GetLatestRawStockData(string stockSymbol)
        {
            return _webProvider.GetLatestRawStockData(string.Format("http://download.finance.yahoo.com/d/quotes.csv?s={0}&f=sl1d1t1c1ohgv&e=.csv", stockSymbol));
        }

        #endregion
    }
}
