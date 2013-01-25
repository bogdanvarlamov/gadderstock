using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.Business
{
    public interface IPurchaseInfo
    {
        decimal PurchasePrice { get; set; }
        uint PurchaseQuantity { get; set; }
        DateTime PurchaseTimestamp { get; set; }
    }
}
