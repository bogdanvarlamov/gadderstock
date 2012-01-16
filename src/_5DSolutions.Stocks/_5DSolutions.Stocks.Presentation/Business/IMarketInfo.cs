using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.Presentation.Business
{
    public interface IMarketInfo: IStock
    {
        decimal MarketPrice { get; set; }
        DateTime Timestamp { get; set; }
    }
}
