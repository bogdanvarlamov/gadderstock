using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using _5DSolutions.Stocks.DataAccess;
using System.Xml.Linq;

namespace _5DSolutions.Stocks.Business
{
    public static class Stocks
    {
        #region Private Members

        static string PATH_TO_SAVE_FILE = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\5DSolutions\RoverStock\App_Data\App_State.xml";

        #endregion//Private Members

        #region Public Methods

        public static IEnumerable<T> GetMarketInfo<T>(string symbol) where T : IMarketInfo, new()
        {
            YahooCSVStockDataProvider _provider = new YahooCSVStockDataProvider();
            YahooCSVConverter<T> _converter = new YahooCSVConverter<T>();

            return _converter.GetStockInfoListFromString(_provider.GetLatestRawStockData(symbol));

        }

        #region Loading/Saving

        public static object LoadSavedStocks(Type deserializationType)
        {
            object toReturn = null;
            XDocument xmlAppStateDoc = null;

            if (System.IO.File.Exists(PATH_TO_SAVE_FILE))
            {//load it

                //TODO: add some schema validation for the document to make sure its properly formatted, etc.
                xmlAppStateDoc = XDocument.Load(PATH_TO_SAVE_FILE);

                //serialize the xml to the appstate to return
                XmlSerializer xs = new XmlSerializer(deserializationType);
                toReturn = xs.Deserialize(xmlAppStateDoc.CreateReader());
            }
            else
            {
                //nothing to load
            }

            return toReturn;
        }

        public static void SaveAllStocks<T>(T objectToSerialize)
        {
            Uri uri = new Uri(PATH_TO_SAVE_FILE);

            if (System.IO.Directory.Exists(uri.LocalPath.Replace(uri.Segments.Last(), string.Empty)))
            {
                //already there, do nothing
            }
            else
            {//make the dir
                System.IO.Directory.CreateDirectory(uri.LocalPath.Replace(uri.Segments.Last(), string.Empty));
            }

            using (System.IO.FileStream tmp = System.IO.File.Create(PATH_TO_SAVE_FILE))
            {
                XmlSerializer xs = new XmlSerializer(objectToSerialize.GetType());
                xs.Serialize(tmp, objectToSerialize);//serialize the stocklist and save it
            }
        }

        #endregion//Loading/Saving

        #endregion//Public Methods
    }
}
