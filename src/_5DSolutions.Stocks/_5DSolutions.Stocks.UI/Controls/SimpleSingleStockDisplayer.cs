using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

using _5DSolutions.Stocks.LowerLevelBusiness;

namespace _5DSolutions.Stocks.UI.Controls
{
    public class SimpleSingleStockDisplayer: Control
    {
        #region Dependency Properties
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register("Symbol", typeof(string), typeof(SimpleSingleStockDisplayer));
        public static readonly DependencyProperty PurchasePriceProperty = DependencyProperty.Register("PurchasePrice", typeof(decimal), typeof(SimpleSingleStockDisplayer));
        public static readonly DependencyProperty PurchaseAmountProperty = DependencyProperty.Register("PurchaseAmount", typeof(uint), typeof(SimpleSingleStockDisplayer));
        public static readonly DependencyProperty CurrentPriceProperty = DependencyProperty.Register("CurrentPrice", typeof(decimal), typeof(SimpleSingleStockDisplayer));
        public static readonly DependencyProperty CurrentPriceTimestampProperty = DependencyProperty.Register("CurrentPriceTimestamp", typeof(DateTime), typeof(SimpleSingleStockDisplayer));

        #endregion//Dependency Properties

        #region Public Properties

        public string Symbol
        {
            get
            {
                return (string)GetValue(SymbolProperty);
            }
            set
            {
                SetValue(SymbolProperty, value);
            }
        }

        public decimal PurchasePrice
        {
            get
            {
                return (decimal)GetValue(PurchasePriceProperty);
            }
            set
            {
                SetValue(PurchasePriceProperty, value);
            }
        }

        public uint PurchaseAmount
        {
            get
            {
                return (uint)GetValue(PurchaseAmountProperty);
            }
            set
            {
                SetValue(PurchaseAmountProperty, value);
            }
        }

        public decimal CurrentPrice
        {
            get
            {
                return (decimal)GetValue(CurrentPriceProperty);
            }
            set
            {
                SetValue(CurrentPriceProperty, value);
            }
        }

        public DateTime CurrentPriceTimestamp
        {
            get
            {
                return (DateTime)GetValue(CurrentPriceTimestampProperty);
            }
            set
            {
                SetValue(CurrentPriceTimestampProperty, value);
            }
        }

        #endregion//Public Properties


        #region Constructor

        public SimpleSingleStockDisplayer(ISimpleStockInfo stockInfo, IPurchaseInfo purchaseInfo)
        {
            BindStockInfo(stockInfo);
            BindPurchaseInfo(purchaseInfo);
        }

        private void BindStockInfo(ISimpleStockInfo stockInfo)
        {
            Binding symbolBinding = new Binding("Symbol");
            symbolBinding.Mode = BindingMode.TwoWay;
            symbolBinding.Source = stockInfo;
            SetBinding(SymbolProperty, symbolBinding);

            Binding priceBinding = new Binding("Price");
            priceBinding.Mode = BindingMode.TwoWay;
            priceBinding.Source = stockInfo;
            SetBinding(CurrentPriceProperty, priceBinding);

            Binding timestampBinding = new Binding("Timestamp");
            timestampBinding.Mode = BindingMode.TwoWay;
            timestampBinding.Source = stockInfo;
            SetBinding(CurrentPriceTimestampProperty, timestampBinding);
        }

        #endregion//Constructor

        public void BindPurchaseInfo(IPurchaseInfo info)
        {
            this.PurchasePrice = info.PurchasePrice;
            this.PurchaseAmount = info.PurchaseAmount;

            Binding purchasePriceBinding = new Binding("PurchasePrice");
            purchasePriceBinding.Mode = BindingMode.TwoWay;
            purchasePriceBinding.Source = info;
            SetBinding(PurchasePriceProperty, purchasePriceBinding);

            Binding purchaseAmountBinding = new Binding("PurchaseAmount");
            purchaseAmountBinding.Mode = BindingMode.TwoWay;
            purchaseAmountBinding.Source = info;
            SetBinding(PurchaseAmountProperty, purchaseAmountBinding);

        }
    }
}
