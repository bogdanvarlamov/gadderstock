using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

using System.ComponentModel;

namespace _5DSolutions.Stocks.LowerLevelBusiness
{
    [XmlRoot("PurchaseData")]
    public class PurchaseInfo: INotifyPropertyChanged, IPurchaseInfo
    {
        #region Private Members

        string _symbol;
        decimal _purchasePrice;
        uint _purchaseAmount;

        #endregion//Private Members

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IPurchaseInfo Members

        [XmlAttribute()]
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    RaisePropertyChanged("Symbol");
                }
            }
        }

        [XmlAttribute()]
        public decimal PurchasePrice
        {
            get
            {
                return _purchasePrice;
            }
            set
            {
                if (_purchasePrice != value)
                {
                    _purchasePrice = value;
                    RaisePropertyChanged("PurchasePrice");
                }
            }
        }
        [XmlAttribute()]
        public uint PurchaseAmount
        {
            get
            {
                return _purchaseAmount;
            }
            set
            {
                if (_purchaseAmount != value)
                {
                    _purchaseAmount = value;
                    RaisePropertyChanged("PurchaseAmount");
                }
            }
        }

        #endregion

        #region Private Helper Methods

        private void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);

                PropertyChanged(this, args);
            }
        }

        #endregion//Private Helper Methods
    }
}
