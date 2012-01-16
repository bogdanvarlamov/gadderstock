using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using _5DSolutions.Stocks.LowerLevelBusiness;


namespace _5DSolutions.Stocks.Business
{

    public class DisplayedStock: INotifyPropertyChanged, IPurchaseInfo, ISimpleStockInfo 
    {
        PurchaseInfo _currentPurchaseInfo = new PurchaseInfo();
        StockInfo _currentStockInfo = new StockInfo();

        //public PurchaseInfo CurrentPurchaseInfo
        //{
        //    get { return _currentPurchaseInfo; }
        //    set
        //    {
        //        if (!value.Equals(_currentPurchaseInfo))
        //        {
        //            _currentPurchaseInfo = value;
        //            RaisePropertyChanged("CurrentPurchaseInfo");
        //        }
        //        else
        //        {
        //            //same thing
        //        }
        //    }
        //}

        //public StockInfo CurrentStockInfo
        //{
        //    get { return _currentStockInfo; }
        //    set
        //    {
        //        if (!value.Equals(_currentStockInfo))
        //        {
        //            _currentStockInfo = value;
        //            RaisePropertyChanged("CurrentStockInfo");
        //        }
        //        else
        //        {
        //            //same thing
        //        }
                

        //    }
        //}


        public DisplayedStock()
        {
            _currentPurchaseInfo.PropertyChanged += (sender, args) =>
                {
                    RaisePropertyChanged(args.PropertyName);
                };
            _currentStockInfo.PropertyChanged += (sender, args) =>
                {
                    RaisePropertyChanged(args.PropertyName);
                };
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChangedEventHandler tmp = PropertyChanged;

            if (null != tmp)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propName);
                tmp(this, args);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IPurchaseInfo Members

        public string Symbol
        {
            get
            {
                return _currentPurchaseInfo.Symbol;
            }
            set
            {
                _currentPurchaseInfo.Symbol = value;
            }
        }

        public decimal PurchasePrice
        {
            get
            {
                return _currentPurchaseInfo.PurchasePrice;
            }
            set
            {
                _currentPurchaseInfo.PurchasePrice = value;
            }
        }

        public uint PurchaseAmount
        {
            get
            {
                return _currentPurchaseInfo.PurchaseAmount;
            }
            set
            {
                _currentPurchaseInfo.PurchaseAmount = value;
            }
        }

        #endregion

        #region ISimpleStockInfo Members

        public decimal Price
        {
            get
            {
                return _currentStockInfo.Price;
            }
            set
            {
                _currentStockInfo.Price = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return _currentStockInfo.Timestamp;
            }
            set
            {
                _currentStockInfo.Timestamp = value;
            }
        }

        #endregion
    }
}
