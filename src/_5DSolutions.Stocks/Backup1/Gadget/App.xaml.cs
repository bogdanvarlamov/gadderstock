using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Collections.ObjectModel;

using _5DSolutions.Stocks.Business;
using _5DSolutions.Stocks.Gadget.UI;

namespace _5DSolutions.Stocks.Gadget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string _relativePathToAppState;
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ////set the path to the app state document
            //_relativePathToAppState = @"\5DSolutions\RoverStock\App_Data\App_State.xml";

            //ApplicationState state = ApplicationStateManager.GetLastAppState(_relativePathToAppState);

            //this.Exit += (exitSender, args) =>
            //    {
            //        ApplicationStateManager.SaveCurrentAppState(state, _relativePathToAppState);
            //    };


            //state.OnStateChanged += () =>
            //    {
            //        ApplicationStateManager.SaveCurrentAppState(state, _relativePathToAppState);
            //    };

            //Docked tmp = new Docked();
            //tmp.SetDisplayedStockInfo(state.DisplayedStocks.AsEnumerable());

            //var symbols = from displayed in state.DisplayedStocks
            //              select displayed.Symbol;

            //YahooCSVPresenter<DisplayedStock> yPres = new YahooCSVPresenter<DisplayedStock>(tmp, 5000, state.DisplayedStocks);
            //yPres.OnStockInfoAdded += (args) =>
            //    {
            //        ApplicationStateManager.SaveCurrentAppState(state, _relativePathToAppState);
            //    };

            //tmp.OnPurchaseInfoAddRequest += (args) =>
            //    {
            //        PurchaseInfoAddResult toReturn = new PurchaseInfoAddResult();

            //        ApplicationState state = ApplicationStateManager.GetLastAppState(_relativePathToAppState);

            //        (from stock in state.DisplayedStocks
            //           where stock.CurrentStockInfo.Symbol.Equals(args.Symbol, StringComparison.InvariantCultureIgnoreCase)
            //           select stock).First().CurrentPurchaseInfo = new PurchaseInfo() { Symbol = args.Symbol };

            //        //TODO: add useful info to return value
            //        return toReturn;
            //    };

            //XmlDocPurchaseInfoPresenter<PurchaseInfo> xPres = new XmlDocPurchaseInfoPresenter<PurchaseInfo>(tmp, new Uri(_uriToAppStateXmlDoc));

            NavigationWindow mainWindow = Application.Current.MainWindow as NavigationWindow;
            mainWindow.Navigate(new Main());

        }
        //private void UpdateAppState(StockInfoAddRequestEventArgs args)
        //{
        //    ApplicationState tmp = ApplicationStateManager.GetLastAppState(_relativePathToAppState);
        //}
        
    }
}
