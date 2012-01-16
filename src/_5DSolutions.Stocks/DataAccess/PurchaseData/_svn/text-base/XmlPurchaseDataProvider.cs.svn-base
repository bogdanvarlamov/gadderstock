using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace _5DSolutions.Stocks.DataAccess.PurchaseData
{
    public class XPurchaseDataProvider
    {

        #region Private Members

        XDocument _xmlPurchaseDataDoc;
        Uri _originatingUri;

        #endregion//Private Members

        #region Constructors

        public XPurchaseDataProvider(Uri uri)
        {
            _originatingUri = uri;

            if (System.IO.File.Exists(uri.AbsolutePath))
            {
                _xmlPurchaseDataDoc = XDocument.Load(uri.ToString());
            }
            else
            {//first time, lets create it...
                if (System.IO.Directory.Exists(_originatingUri.AbsolutePath))
                {
                    //already there, the file is just gone...
                }
                else
                {
                    System.IO.Directory.CreateDirectory(_originatingUri.AbsolutePath.Replace(_originatingUri.Segments.Last(), string.Empty));
                }

                using (System.IO.FileStream tmp = System.IO.File.Create(_originatingUri.AbsolutePath))
                {
                    //do nothing...
                }
                

                _xmlPurchaseDataDoc = new XDocument(new XElement("Purchases"));
                _xmlPurchaseDataDoc.Save(_originatingUri.AbsolutePath);
                
            }

            //TODO: add some schema validation for the document to make sure its properly formatted, etc.
        }

        #endregion//Constructors

        #region Public Methods

        public XElement GetPurchaseData(string stockSymbol)
        {
            XElement toReturn = null;

            lock (_xmlPurchaseDataDoc)
            {
                
                var matchingElements = (from curElement in _xmlPurchaseDataDoc.Elements("PurchaseData")
                           where curElement.Attribute("Symbol").Value.Equals(stockSymbol, StringComparison.InvariantCultureIgnoreCase)
                           select curElement);
                if (matchingElements.Count() > 0)
                {
                    toReturn = matchingElements.First();
                }
                else
                {
                    //no element found..
                }
            }

            return toReturn;
        }

        public IEnumerable<XElement> GetAllPurchaseData()
        {
            return _xmlPurchaseDataDoc.Root.Elements("PurchaseData");
        }

        #endregion//Public Methods

        public void SetPurchaseData(XElement elementToAdd)
        {
            lock (_xmlPurchaseDataDoc)
            {
                var alreadyThere = from curElement in _xmlPurchaseDataDoc.Root.Elements()
                                   where curElement.Attribute("Symbol").Value.Equals(elementToAdd.Attribute("Symbol").Value, StringComparison.InvariantCultureIgnoreCase)
                                   select curElement;

                if (alreadyThere.Count() > 0)
                {
                    foreach (XElement xelem in alreadyThere)
                    {
                        xelem.ReplaceWith(elementToAdd);
                    }
                }
                else
                {
                    _xmlPurchaseDataDoc.Root.Add(elementToAdd);
                }
                _xmlPurchaseDataDoc.Save(_originatingUri.AbsolutePath);
            }
        }
    }
}
