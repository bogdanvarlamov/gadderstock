using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace _5DSolutions.Stocks.LowerLevelBusiness
{
    public class StockInfo: INotifyPropertyChanged, ISimpleStockInfo 
    {
        #region Private Variables

        string _symbol;
        decimal _price;
        DateTime _timestamp; 

        #endregion//Private Variables

        #region Public Properties

        #region ISimpleStockInfo Members



        public string Symbol
        {
            get { return _symbol; }
            set 
            {
                if (value != _symbol)
                {
                    _symbol = value;
                    RaisePropertyChanged("Symbol");
                }
            }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value != _price)
                {
                    _price = value;
                    RaisePropertyChanged("Price");
                }
            }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set
            {
                if (value != _timestamp)
                {
                    _timestamp = value;
                    RaisePropertyChanged("Timestamp");
                }
            }
        }
        #endregion//ISimpleStockInfo Members

        #endregion//Public Properties

        #region Consturctors

        public StockInfo()
        {
            //do nothing
        }

        public StockInfo(string symbol, decimal price, DateTime timestamp)
            : this()
        {
            _symbol = symbol;
            _price = price;
            _timestamp = timestamp;
        }

        #endregion//Consturctors


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        } 
        #endregion//Private Helpers


    }
}
