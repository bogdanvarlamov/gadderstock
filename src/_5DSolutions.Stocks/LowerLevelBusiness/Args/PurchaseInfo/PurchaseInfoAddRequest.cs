using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.LowerLevelBusiness
{
    public class PurchaseInfoAddRequestEventArgs
    {
        string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }



        public PurchaseInfoAddRequestEventArgs(string symbol)
        {
            _symbol = symbol;
        }
    }
}
