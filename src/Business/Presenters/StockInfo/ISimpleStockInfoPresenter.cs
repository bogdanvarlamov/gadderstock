using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _5DSolutions.Stocks.DataAccess;
using _5DSolutions.Stocks.LowerLevelBusiness;
using _5DSolutions.Stocks.LowerLevelBusiness.DataConverters;

namespace _5DSolutions.Stocks.Business.Presenters.StockInfo
{
    interface ISimpleStockInfoPresenter<T> where T: ISimpleStockInfo, new()
    {
        string Name { get;}
        string Description { get;}

        event Action<StockInfoAddRequestEventArgs> OnStockInfoAdded;
        event Action<StockInfoRemoveRequestEventArgs> OnStockInfoRemoved;

        bool Bind(IStockDataView<T> view);
        bool UnBind();


    }
}
