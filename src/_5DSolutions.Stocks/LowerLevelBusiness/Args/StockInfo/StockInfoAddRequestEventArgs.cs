using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.LowerLevelBusiness
{
    public class StockInfoAddRequestEventArgs
    {
        string _symbolToAdd;

        public string SymbolToAdd
        {
            get { return _symbolToAdd; }
            set { _symbolToAdd = value; }
        }

        public StockInfoAddRequestEventArgs(string symbolToAdd)
        {
            _symbolToAdd = symbolToAdd;

        }
    }
}
