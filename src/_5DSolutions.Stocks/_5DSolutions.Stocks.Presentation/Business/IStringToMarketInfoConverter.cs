using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.Presentation.Business
{
    public interface IStringToMarketInfoConverter<T> where T : IMarketInfo, new()
    {
        IEnumerable<T> GetStockInfoListFromString(object stringObject);
    }
}
