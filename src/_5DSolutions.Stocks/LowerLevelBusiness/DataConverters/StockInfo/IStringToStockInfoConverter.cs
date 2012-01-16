using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.LowerLevelBusiness.DataConverters
{
    public interface IStringToSimpleStockInfoConverter<T> where T: ISimpleStockInfo, new()
    {
        List<T> GetStockInfoListFromString(object stringObject);
    }
}
