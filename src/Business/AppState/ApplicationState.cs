using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace _5DSolutions.Stocks.Business.AppState
{
    public class ApplicationState
    {
        public event Action OnStateChanged;
        
        
        ObservableCollection<DisplayedStock> _displayedStocks = new ObservableCollection<DisplayedStock>();



        public ObservableCollection<DisplayedStock> DisplayedStocks
        {
            get { return _displayedStocks; }
        }
        object _currentView = null;


        public object CurrentView
        {
            get
            {
                return _currentView;
            }
        }

        public ApplicationState()
        {
            //used for serialization
        }

        public ApplicationState(IEnumerable<DisplayedStock> displayedStocks)
        {
            foreach (DisplayedStock curStock in displayedStocks)
            {
                _displayedStocks.Add(curStock);
                curStock.PropertyChanged += (sender, args) =>
                    {
                        RaiseOnStateChanged();
                    };

                _displayedStocks.CollectionChanged += (sender, args) =>
                    {
                        foreach (DisplayedStock newStock in args.NewItems)
                        {
                            newStock.PropertyChanged += (colsender, colargs) =>
                            {
                                RaiseOnStateChanged();
                            };
                        }

                        RaiseOnStateChanged();
                    };
            }
        }

        private void RaiseOnStateChanged()
        {
            if (null != OnStateChanged)
            {
                OnStateChanged();
            }
        }
    }
}
