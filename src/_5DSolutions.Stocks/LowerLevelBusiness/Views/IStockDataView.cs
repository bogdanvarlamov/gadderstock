using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.LowerLevelBusiness
{
    public interface IStockDataView<T> where T: ISimpleStockInfo, new()
    {
        //void SetSimpleStockInfo(ObservableCollection<ISimpleStockInfo> infoToSet);
        event Func<StockInfoRefreshRequestEventArgs, StockInfoRefreshResult> OnStockInfoRefreshRequest;
        event Func<StockInfoAddRequestEventArgs, StockInfoAddResult> OnStockInfoAddRequest;
        event Func<StockInfoRemoveRequestEventArgs, StockInfoRemoveResult> OnStockInfoRemoveRequest;

        void SetDisplayedStockInfo(IEnumerable<T> stocksList);
        void SetDisplayedStockInfo<J>(IEnumerable<T> stockList, IEnumerable<J> purchaseInfoList) where J : IPurchaseInfo;
    }
}
