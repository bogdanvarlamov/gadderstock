using System;
namespace _5DSolutions.Stocks.LowerLevelBusiness
{
    public interface ISimpleStockInfo
    {
        decimal Price { get; set; }
        string Symbol { get; set; }
        DateTime Timestamp { get; set; }
    }
}
