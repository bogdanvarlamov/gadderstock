using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.ObjectModel;

using _5DSolutions.Stocks.Business;
using _5DSolutions.Stocks.LowerLevelBusiness;
using _5DSolutions.Stocks.UI.Controls;

namespace _5DSolutions.Stocks.Gadget.UI
{
    /// <summary>
    /// Interaction logic for Docked.xaml
    /// </summary>
    public partial class Docked : Page, IStockDataView<DisplayedStock>, IPurchaseInfoEditView
    {

        //NOTE: need to keep track of this... and update/remove as necessary...
        ObservableCollection<SimpleSingleStockDisplayer> _displayers = new ObservableCollection<SimpleSingleStockDisplayer>();

        public Docked()
        {
            InitializeComponent();
            DataContext = _displayers;//NOTE: this is what the list will use to bind against
        }




        #region ISimpleStockDataView<StockInfo> Members

        public void SetDisplayedStockInfo(IEnumerable<DisplayedStock> displayedStocks)
        {
            lock (_displayers)
            {
                foreach (DisplayedStock curInfo in displayedStocks)
                {
                    if (null != curInfo
                     && null != curInfo.Symbol)
                    {
                        if (_displayers.Any(displayer => displayer.Symbol.Equals(curInfo.Symbol, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            //this one already exists
                        }
                        else
                        {
                            _displayers.Add(new SimpleSingleStockDisplayer(curInfo, curInfo));//this will notify and update the UI through the binding engine in WPF
                        }
                    }
                    else
                    {
                        //its null
                    }
                }
            }
        }

        public void SetDisplayedStockInfo<J>(IEnumerable<DisplayedStock> stockList, IEnumerable<J> purchaseInfoList) where J : IPurchaseInfo
        {
            throw new NotImplementedException();
        }



        #region Events

        public event Func<StockInfoRefreshRequestEventArgs, StockInfoRefreshResult> OnStockInfoRefreshRequest;
        public event Func<StockInfoAddRequestEventArgs, StockInfoAddResult> OnStockInfoAddRequest;
        public event Func<StockInfoRemoveRequestEventArgs, StockInfoRemoveResult> OnStockInfoRemoveRequest;

        #endregion//Events

        #endregion//ISimpleStockDataView<StockInfo> Members



        private void RaiseOnStockInfoRefreshRequest()
        {
            if (null != OnStockInfoRefreshRequest)
            {

                OnStockInfoRefreshRequest(new StockInfoRefreshRequestEventArgs());

            }
        }

        private void RaiseOnStockInfoAddRequest(StockInfoAddRequestEventArgs e)
        {

            if (null != OnStockInfoAddRequest)
            {
                OnStockInfoAddRequest(e);
            }
        }

        private void RaiseOnStockInfoRemoveRequest(StockInfoRemoveRequestEventArgs e)
        {
            if (null != OnStockInfoRemoveRequest)
            {
                OnStockInfoRemoveRequest(e);
            }
        }


        private void uiRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseOnStockInfoRefreshRequest();
        }

        private void uiAddSymbolButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewStockSymbol();
        }

        private void AddNewStockSymbol()
        {
            string symbolToAdd = uiAddSymbolTextBox.Text.ToUpper();

            if (string.Empty != symbolToAdd)
            {
                RaiseOnStockInfoAddRequest(new StockInfoAddRequestEventArgs(symbolToAdd));
                RaiseOnPurchaseInfoAddRequest(new PurchaseInfoAddRequestEventArgs(symbolToAdd));

            }
            else
            {
                //nothing there...
            }

            uiAddSymbolTextBox.Text = string.Empty;
        }


        private void uiAddSymbolTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                AddNewStockSymbol();
            }
        }


        #region IPurchaseInfoEditView Members


        #region Events

        public event Func<PurchaseInfoAddRequestEventArgs, PurchaseInfoAddResult> OnPurchaseInfoAddRequest;


        private void RaiseOnPurchaseInfoAddRequest(PurchaseInfoAddRequestEventArgs e)
        {
            if (null != OnPurchaseInfoAddRequest)
            {
                OnPurchaseInfoAddRequest(e);
            }
        }


        #endregion//Events

        #region Methods

        public void SetPurchaseInfo(IEnumerable<IPurchaseInfo> info)
        {
            lock (_displayers)
            {
                foreach (IPurchaseInfo curInfo in info)
                {
                    var matchingDisplayer = from curDisp in _displayers
                                            where curDisp.Symbol.Equals(curInfo.Symbol, StringComparison.InvariantCultureIgnoreCase)
                                            select curDisp;

                    if (matchingDisplayer.Count() > 0)
                    {
                        //should only be one... but just in case
                        foreach (SimpleSingleStockDisplayer curDisp in matchingDisplayer)
                        {
                            curDisp.BindPurchaseInfo(curInfo);
                        }
                    }
                    else
                    {
                        //there isn't one, its a purchase info for a stock to which we are not receiving updates about
                    }
                }
            }
        }

        #endregion//Methods

        #endregion//IPurchaseInfoEditView Members




    }
}
