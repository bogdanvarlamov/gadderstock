using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.LowerLevelBusiness
{
    public class StockInfoRemoveRequestEventArgs
    {
        string _symbolToRemove;

        public string SymbolToRemove
        {
            get { return _symbolToRemove; }
            set { _symbolToRemove = value; }
        }

        public StockInfoRemoveRequestEventArgs(string symbolToAdd)
        {
            _symbolToRemove = symbolToAdd;

        }
    }
}
