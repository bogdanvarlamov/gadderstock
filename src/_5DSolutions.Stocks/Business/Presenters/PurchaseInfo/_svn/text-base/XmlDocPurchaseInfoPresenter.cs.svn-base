using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Linq;

using _5DSolutions.Stocks.DataAccess.PurchaseData;
using _5DSolutions.Stocks.LowerLevelBusiness;
using _5DSolutions.Stocks.LowerLevelBusiness.DataConverters.PurchaseInfo;

namespace _5DSolutions.Stocks.Business.Presenters.PurchaseInfo
{
    public class XmlDocPurchaseInfoPresenter<T> where T: IPurchaseInfo, INotifyPropertyChanged,  new()
    {
        //XElementToPurchaseInfoConverter<T> _xConverter;
        //XPurchaseDataProvider _xDataProvider;
        IPurchaseInfoEditView _view;

        public event Func<PurchaseInfoAddRequestEventArgs, PurchaseInfoAddResult> OnPurchaseInfoAddRequest;

        ObservableCollection<IPurchaseInfo> _purchaseInfoList = new ObservableCollection<IPurchaseInfo>();

        public XmlDocPurchaseInfoPresenter(IPurchaseInfoEditView view, IEnumerable<IPurchaseInfo> startingInfos)
        {
            //_xDataProvider = new XPurchaseDataProvider(uriToXmlDoc);
            //_xConverter = new XElementToPurchaseInfoConverter<T>();
            _view = view;

            _purchaseInfoList.CollectionChanged += (sender, args) =>
                {//this will happen when new items are added/removed
                    //foreach (INotifyPropertyChanged curItem in args.NewItems)
                    //{
                    //    curItem.PropertyChanged += (item, itemArgs) =>
                    //    {//when those items have an update, notify whoever
                    //        //SavePurchaseInfoToXDoc((IPurchaseInfo)item);
                    //        RaisePurchaseInfoItemUpdated();
                    //    };
                    //}
                };

            foreach (IPurchaseInfo tmpPurchaseInfo in startingInfos)
            {
                _purchaseInfoList.Add(tmpPurchaseInfo);//add them to the list, this causes update notifications
            }

            

            _view.SetPurchaseInfo(_purchaseInfoList);//tell the view to add them
            _view.OnPurchaseInfoAddRequest += (args) =>
                {
                    return AddPurchaseInfo(args);
                };
        }



        private PurchaseInfoAddResult AddPurchaseInfo(PurchaseInfoAddRequestEventArgs e)
        {
            PurchaseInfoAddResult toReturn = new PurchaseInfoAddResult();
            T toAdd = new T();

            lock (_purchaseInfoList)
            {

                var alreadyThere = from curPurchaseInfo in _purchaseInfoList
                                   where curPurchaseInfo.Symbol.Equals(e.Symbol, StringComparison.InvariantCultureIgnoreCase)
                                   select curPurchaseInfo;

                if (alreadyThere.Count() > 0)
                {//there is one already there... 
                    //do nothing, it is already added
                }
                else
                {//need to add a new one!
                    toAdd.Symbol = e.Symbol;
                    _purchaseInfoList.Add(toAdd);

                    //need to save it to the xmldoc
                    toReturn = RaiseOnPurchaseInfoAddRequest(e);
                }
            }

            _view.SetPurchaseInfo(_purchaseInfoList);

            return toReturn;
        }

        //private PurchaseInfoAddResult RaiseOnPurchaseInfoAddRequest(PurchaseInfoAddRequestEventArgs e)
        //{
            
        //}

        //private void SavePurchaseInfoToXDoc(IPurchaseInfo toSave)
        //{
        //    //need to get the XElement from the object
        //    XElement elementToAdd = _xConverter.ConvertFrom(toSave) as XElement;

        //    _xDataProvider.SetPurchaseData(elementToAdd);

        //}
        private PurchaseInfoAddResult RaiseOnPurchaseInfoAddRequest(PurchaseInfoAddRequestEventArgs args)
        {
            PurchaseInfoAddResult toReturn = new PurchaseInfoAddResult();

            if (null != OnPurchaseInfoAddRequest)
            {
                OnPurchaseInfoAddRequest(args);
            }

            return toReturn;
        }



    }
}
