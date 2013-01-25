using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Xml.Linq;

using MvvmFoundation.Wpf;
using _5DSolutions.Stocks.Business;


namespace _5DSolutions.Stocks.Presentation.Wpf.ViewModels
{
    public class StockPresenterViewModel: ObservableObject
    {
        #region Private Memebers

        RelayCommand _addNewStockCommand;
        System.Threading.Timer _dataRefreshTimer = null;
        uint _dataRefreshInterval = 5000u;
        string _newStockSymbol = string.Empty;
        #endregion

        #region Commands

        public ICommand AddNewStockCommand
        {
            get
            {
                return _addNewStockCommand ?? ( _addNewStockCommand = new RelayCommand(() =>  CreateStockFromSymbolName()));
            }
        }

        #endregion//Commands

        #region Observable Properties

        public uint DataRefreshInterval
        {
            get
            {
                return _dataRefreshInterval;
            }
            set
            {
                _dataRefreshInterval = value;
            }
        }

        public string NewStockSymbol
        {
            get { return _newStockSymbol; }
            set
            {
                if (value != _newStockSymbol)
                {
                    _newStockSymbol = value;

                    base.RaisePropertyChanged("NewStockSymbol");
                }
            }
        }

        public ObservableCollection<StockViewModel> Stocks { get; private set; }

        #endregion//Observable Properties

        #region Constructors

        public StockPresenterViewModel()
        {

            Stocks = GetSavedStocks();

            _dataRefreshTimer = new System.Threading.Timer((state) =>
            {
                _dataRefreshTimer.Change(System.Threading.Timeout.Infinite, _dataRefreshInterval);

                if (null != Stocks)
                {
                    lock (Stocks)
                    {
                        foreach (StockViewModel curStock in Stocks)
                        {
                            //assuming it is the first one
                            var infos = Business.Stocks.GetMarketInfo<StockViewModel>(curStock.Symbol);
                            if (infos.Count() > 0)
                            {
                                StockViewModel tmp = infos.First();
                                curStock.Timestamp = tmp.Timestamp;
                                curStock.MarketPrice = tmp.MarketPrice;
                            }
                        }
                    }
                }

                _dataRefreshTimer.Change(_dataRefreshInterval, _dataRefreshInterval);
            },
            null,
            0,
            _dataRefreshInterval);
        }

        #endregion//Constructors

        #region Private Helper Methods

        private void CreateStockFromSymbolName()
        {
            if (!string.IsNullOrEmpty(NewStockSymbol))
            {//make sure there is something there

                if (null != Stocks)
                {
                    lock (Stocks)
                    {
                        if (!Stocks.Any(stock => stock.Symbol.Equals(_newStockSymbol, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            foreach (StockViewModel curStock in Business.Stocks.GetMarketInfo<StockViewModel>(_newStockSymbol))
                            {
                                AddEventHandlers(curStock);

                                Stocks.Add(curStock);
                            }

                            SaveAllStocks();
                        }
                    }
                }

                NewStockSymbol = string.Empty;//clear the new stock
            }
        }

        private void AddEventHandlers(StockViewModel curStock)
        {
            curStock.OnDeleted += () =>
            {
                lock (Stocks)
                {
                    Stocks.Remove(curStock);
                }
                SaveAllStocks();
            };

            curStock.OnSaved += () =>
            {
                SaveAllStocks();
            };
        }

        private ObservableCollection<StockViewModel> GetSavedStocks()
        {
            ObservableCollection<StockViewModel> toReturn = new ObservableCollection<StockViewModel>();

            toReturn = Business.Stocks.LoadSavedStocks(toReturn.GetType()) as ObservableCollection<StockViewModel>;

            foreach (StockViewModel curStock in toReturn)
            {//subscribe to the events
                AddEventHandlers(curStock);
            }

            return toReturn;
        }
        private void SaveAllStocks()
        {
            Business.Stocks.SaveAllStocks <ObservableCollection<StockViewModel>> (Stocks);
        }

        #endregion//Private Helper Methods
    }
}
