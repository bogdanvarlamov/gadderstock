using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MvvmFoundation.Wpf;
using _5DSolutions.Stocks.Business;

namespace _5DSolutions.Stocks.Presentation.Wpf.ViewModels
{
    public class StockViewModel: ObservableObject, IMarketInfo, IPurchaseInfo
    {

        #region Observable Properties

        #region IMarketInfo Members

        public decimal MarketPrice
        {
            get
            {
                return _marketPrice;
            }
            set
            {
                if (value != _marketPrice)
                {
                    _marketPrice = value;
                    base.RaisePropertyChanged("MarketPrice");
                    base.RaisePropertyChanged("ReturnOnInvestmentPercent");
                }
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return _timestamp;
            }
            set
            {
                if (value != _timestamp)
                {
                    _timestamp = value;
                    base.RaisePropertyChanged("Timestamp");
                }
            }
        }

        #endregion

        #region IStock Members

        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                if (value != _symbol)
                {
                    _symbol = value;
                    base.RaisePropertyChanged("Symbol");
                }
            }
        }

        #endregion

        #region IPurchaseInfo Members

        public decimal PurchasePrice
        {
            get
            {
                return _purchasePrice;
            }
            set
            {
                if (value != _purchasePrice)
                {
                    _purchasePrice = value;
                    base.RaisePropertyChanged("PurchasePrice");
                    base.RaisePropertyChanged("TotalPurchaseCost");
                    base.RaisePropertyChanged("ReturnOnInvestmentPercent");
                }
            }
        }

        public uint PurchaseQuantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value != _quantity)
                {
                    _quantity = value;
                    base.RaisePropertyChanged("PurchaseQuantity");
                    base.RaisePropertyChanged("TotalPurchaseCost");
                    base.RaisePropertyChanged("TotalMarketValue");
                    base.RaisePropertyChanged("ReturnOnInvestmentPercent");
                }
            }
        }

        public DateTime PurchaseTimestamp
        {
            get
            {
                return _purchaseTimestamp;
            }
            set
            {
                if (value != _purchaseTimestamp)
                {
                    _purchaseTimestamp = value;
                    base.RaisePropertyChanged("PurchaseTimestamp");
                }
            }
        }

        #endregion

        #region Calculated

        public decimal TotalPurchaseCost
        {
            get
            {
                return PurchasePrice * PurchaseQuantity;
            }
        }

        public decimal TotalMarketValue
        {
            get
            {
                return MarketPrice * PurchaseQuantity;
            }
        }

        public decimal ReturnOnInvestmentPercent
        {
            get
            {
                return TotalMarketValue / TotalPurchaseCost;
            }
        }

        #endregion//Calculated

        #endregion//Observable Properties

        #region Private Members

        private decimal _marketPrice;
        private DateTime _timestamp;
        private string _symbol;
        private decimal _purchasePrice;
        private uint _quantity;
        private DateTime _purchaseTimestamp;

        RelayCommand _deleteCommand;
        RelayCommand _saveCommand;

        #endregion//Private Members

        #region Events

        public event Action OnDeleted;
        public event Action OnSaved;

        #endregion//Events

        #region Commands

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand(OnDeleted, () => null != OnDeleted));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(OnSaved, () => null != OnSaved));
            }
        }

        #endregion// Commands

        #region Constructors

        public StockViewModel()
        {

        }

        #endregion// Constructors
    }
}
