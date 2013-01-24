using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;
using _5DSolutions.Stocks.DataAccess;
using _5DSolutions.Stocks.LowerLevelBusiness;
using _5DSolutions.Stocks.LowerLevelBusiness.DataConverters;

namespace _5DSolutions.Stocks.Business.Presenters.StockInfo
{
    public class YahooCSVPresenter<T> : ISimpleStockInfoPresenter<T> where T : ISimpleStockInfo, new()
    {
        #region Private Variables

        string _name = "Yahoo!";
        string _description = "Uses the Yahoo! website to present stocks information.";

        IStockDataView<T> _view;
        bool _isViewBound = false;
        Mutex _viewBindingMutex = new Mutex();

        ObservableCollection<T> _stocksList = null;

        public IEnumerable<T> StocksList
        {
            get { return _stocksList; }
            //set { _stocksList = value; }
        }

        YahooCSVStockDataProvider _provider;
        YahooCSVStockDataConverter<T> _converter;

        System.Threading.Timer _dataRefreshTimer;
        int _dataRefreshIntervalInMS;

        #endregion//Private Variables

        #region IStockPresenter Members

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public bool Bind(IStockDataView<T> viewToBind)
        {
            bool toReturn = false; //assume failure

            try
            {
                _viewBindingMutex.WaitOne();

                if (_isViewBound)
                {
                    UnBind();
                }
                else
                {
                    //no view is bound
                }

                _view = viewToBind;
                //_view.SetSimpleStockInfo(_stocksList);

                _view.OnStockInfoRefreshRequest += (args) =>
                    {
                        StockInfoRefreshResult refreshResult = new StockInfoRefreshResult();//TODO: implement object
                        UpdateStocks(null);
                        return refreshResult;
                    };

                _view.OnStockInfoAddRequest += (args) =>
                    {
                        StockInfoAddResult addResult = new StockInfoAddResult();
                        AddNewStockInfo(args);
                        RaiseOnStockInfoAddRequest(args);
                        return addResult;
                    };

                _isViewBound = true;


                toReturn = true;//indicate success
            }
            catch
            {
                //TODO: think about exception handling, for now, eat it
                toReturn = false;//indicate failure
            }
            finally
            {
                try
                {
                    _viewBindingMutex.ReleaseMutex();
                }
                catch
                {
                    //alread released elsewhere!!?
                }
            }

            return toReturn;
        }

        private void RaiseOnStockInfoAddRequest(StockInfoAddRequestEventArgs args)
        {
            if (null != OnStockInfoAdded)
            {
                OnStockInfoAdded(args);
            }
        }
        private void AddNewStockInfo(StockInfoAddRequestEventArgs e)
        {

            T tmp = new T();
            tmp.Symbol = e.SymbolToAdd;

            lock (_stocksList)
            {
                if (_stocksList.Any<T>(stock => stock.Symbol.Equals(tmp.Symbol, StringComparison.InvariantCultureIgnoreCase)))
                {
                    //ignore
                }
                else
                {
                    _stocksList.Add(tmp);
                }
            }

            if (null != _view)
            {
                lock (_view)
                {
                    _view.SetDisplayedStockInfo(_stocksList);
                }

                UpdateStocks(null);
            }

        }


        public bool UnBind()
        {
            bool toReturn = false;//assume failure

            try
            {
                _viewBindingMutex.WaitOne();

                //TODO: add event unregistering and object clearing

                toReturn = true;//indicate success
            }
            catch
            {
                //TODO: think about exception handling
                toReturn = false;
            }
            finally
            {
                try
                {
                    _viewBindingMutex.ReleaseMutex();
                }
                catch
                {
                    //alread released elsewhere!!?
                }
            }

            return toReturn;
        }

        #endregion

        public YahooCSVPresenter(IStockDataView<T> viewToBind, int dataRefreshIntervalInMS, ObservableCollection<T> stockSymbols)
        {

            if (null != stockSymbols)
            {
                _dataRefreshIntervalInMS = dataRefreshIntervalInMS;
                _dataRefreshTimer = new Timer(new TimerCallback(UpdateStocks));
                _provider = new YahooCSVStockDataProvider();
                _converter = new YahooCSVStockDataConverter<T>();
                _stocksList = stockSymbols; //GetStockInfo(stockSymbols);
                Bind(viewToBind);

                ReStartDataRefreshTimer();
            }
            else
            {
                throw new ArgumentNullException("stockSymbols");
            }


        }

        private void UpdateStocks(object state)
        {
            StopDataRefreshTimer();

            lock (_stocksList)
            {
                foreach (T curStock in _stocksList)
                {
                    List<T> tmp = _converter.GetStockInfoListFromString(_provider.GetLatestRawStockData(curStock.Symbol)) as List<T>;
                    if (null != tmp)
                    {
                        foreach (T updatedStock in tmp)//NOTE: there should really only be one here... but the converter returns a list, so just to be safe...
                        {
                            if (updatedStock.Symbol == curStock.Symbol)
                            {
                                curStock.Price = updatedStock.Price;
                                curStock.Timestamp = updatedStock.Timestamp;
                            }
                            else
                            {
                                //something else
                            }
                        }

                    }

                }
            }

            ReStartDataRefreshTimer();
        }

        private void ReStartDataRefreshTimer()
        {

            if (null != _dataRefreshTimer)
            {
                lock (_dataRefreshTimer)
                {
                    _dataRefreshTimer.Change(_dataRefreshIntervalInMS, _dataRefreshIntervalInMS);
                }
            }
        }

        private void StopDataRefreshTimer()
        {
            if (null != _dataRefreshTimer)
            {
                lock (_dataRefreshTimer)
                {
                    _dataRefreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
        }

        //private List<T> GetStockInfo(string[] symbols)
        //{
        //    List<T> toReturn = new List<T>();

        //    foreach (string curStockSymbol in symbols)
        //    {
        //        List<T> tmp = _converter.GetStockInfoListFromString(_provider.GetLatestRawStockData(curStockSymbol)) as List<T>;
        //        if (null != tmp)
        //        {
        //            toReturn.AddRange(tmp);
        //        }
        //    }

        //    return toReturn;
        //}


        #region ISimpleStockInfoPresenter<T> Members


        public event Action<StockInfoAddRequestEventArgs> OnStockInfoAdded;

        #endregion

        #region ISimpleStockInfoPresenter<T> Members


        public event Action<StockInfoRemoveRequestEventArgs> OnStockInfoRemoved;

        #endregion
    }
}
