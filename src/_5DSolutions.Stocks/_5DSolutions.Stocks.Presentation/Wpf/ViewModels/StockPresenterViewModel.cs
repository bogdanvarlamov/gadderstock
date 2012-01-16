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
using _5DSolutions.Stocks.DataAccess;

namespace _5DSolutions.Stocks.Presentation.Wpf.ViewModels
{
    public class StockPresenterViewModel: ObservableObject
    {
        #region Private Memebers

        YahooCSVStockDataProvider _provider;
        YahooCSVConverter<StockViewModel> _converter;

        string _pathToSaveFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\5DSolutions\RoverStock\App_Data\App_State.xml";
        uint _dataRefreshInterval = 5000;
        string _newStockSymbol = string.Empty;

        System.Threading.Timer _dataRefreshTimer;

        RelayCommand _addNewStockCommand;

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
            _provider = new YahooCSVStockDataProvider();
            _converter = new YahooCSVConverter<StockViewModel>();
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
                            var infos = _converter.GetStockInfoListFromString(_provider.GetLatestRawStockData(curStock.Symbol));
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
                            foreach (StockViewModel curStock in _converter.GetStockInfoListFromString(_provider.GetLatestRawStockData(_newStockSymbol)))
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
            Uri uri = new Uri(_pathToSaveFile);
            ObservableCollection<StockViewModel> toReturn = null;
            XDocument xmlAppStateDoc = null;

            if (System.IO.File.Exists(_pathToSaveFile))
            {//load it

                //TODO: add some schema validation for the document to make sure its properly formatted, etc.
                xmlAppStateDoc = XDocument.Load(_pathToSaveFile);

                //serialize the xml to the appstate to return
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<StockViewModel>));
                toReturn = xs.Deserialize(xmlAppStateDoc.CreateReader()) as ObservableCollection<StockViewModel>;

                foreach (StockViewModel curStock in toReturn)
                {//subscribe to the events
                    AddEventHandlers(curStock);
                }
            }
            else
            {//first time, lets create it...
                if (System.IO.Directory.Exists(uri.LocalPath.Replace(uri.Segments.Last(), string.Empty)))
                {
                    //already there, the file is just gone...
                }
                else
                {//make the dir
                    System.IO.Directory.CreateDirectory(uri.LocalPath.Replace(uri.Segments.Last(), string.Empty));
                }

                //create a new appstate
                toReturn = new ObservableCollection<StockViewModel>();

                SaveAllStocks();

            }

            return toReturn;
        }
        private void SaveAllStocks()
        {

            using (System.IO.FileStream tmp = System.IO.File.Create(_pathToSaveFile))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<StockViewModel>));
                xs.Serialize(tmp, Stocks);//serialize the stocklist and save it
            }
        }

        #endregion//Private Helper Methods
    }
}
