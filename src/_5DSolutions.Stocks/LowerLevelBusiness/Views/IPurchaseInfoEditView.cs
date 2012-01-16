using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.LowerLevelBusiness
{
    public interface IPurchaseInfoEditView
    {
        void SetPurchaseInfo(IEnumerable<IPurchaseInfo> info);

        event Func<PurchaseInfoAddRequestEventArgs, PurchaseInfoAddResult> OnPurchaseInfoAddRequest;
    }
}
