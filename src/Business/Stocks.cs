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
        static string PORTFOLIO_ID_SAVE_FILE = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\5DSolutions\RoverStock\App_Data\portfolio_id.xml";
        static Guid _portfolioId = CreatePortfolioIdentity();

        #endregion//Private Members

        #region Public Methods

        public static IEnumerable<T> GetMarketInfo<T>(string symbol) where T : IMarketInfo, new()
        {
            IEnumerable<T> toReturn = null;
            YahooCSVStockDataProvider _provider = new YahooCSVStockDataProvider();
            YahooCSVConverter<T> _converter = new YahooCSVConverter<T>();

            toReturn = _converter.GetStockInfoListFromString(_provider.GetLatestRawStockData(symbol));
            if (toReturn.Count() > 0)
            {
                RecordHistoricalPrice(symbol, toReturn.First());
            }
            return toReturn;
        }

        public static object LoadSavedStocks(Type deserializationType)
        {
            return DeserializeFileSafely(deserializationType, PATH_TO_SAVE_FILE);
        }

        public static void SaveAllStocks<T>(T objectToSerialize)
        {
            SerializeToFileSafely<T>(objectToSerialize, PATH_TO_SAVE_FILE);
        }


        #endregion//Public Methods

        #region Private Helper Methods

        #region Loading/Saving


        private static object DeserializeFileSafely(Type deserializationType, string pathToFile)
        {
            object toReturn = null;
            XDocument xmlAppStateDoc = null;

            if (System.IO.File.Exists(pathToFile))
            {//load it

                //TODO: add some schema validation for the document to make sure its properly formatted, etc.
                xmlAppStateDoc = XDocument.Load(pathToFile);

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


        private static void SerializeToFileSafely<T>(T objectToSerialize, string pathToFile)
        {
            Uri uri = new Uri(pathToFile);

            if (System.IO.Directory.Exists(uri.LocalPath.Replace(uri.Segments.Last(), string.Empty)))
            {
                //already there, do nothing
            }
            else
            {//make the dir
                System.IO.Directory.CreateDirectory(uri.LocalPath.Replace(uri.Segments.Last(), string.Empty));
            }

            using (System.IO.FileStream tmp = System.IO.File.Create(pathToFile))
            {
                XmlSerializer xs = new XmlSerializer(objectToSerialize.GetType());
                xs.Serialize(tmp, objectToSerialize);//serialize the stocklist and save it
            }
        }

        #endregion//Loading/Saving

        private static Guid CreatePortfolioIdentity()
        {
            Guid toReturn = Guid.NewGuid(); //assume there isn't one saved, or it got wiped out or w/e

            object tmp = DeserializeFileSafely(toReturn.GetType(), PORTFOLIO_ID_SAVE_FILE);

            if (null != tmp)
                toReturn = (Guid)tmp;
            else
                SerializeToFileSafely<Guid>(toReturn, PORTFOLIO_ID_SAVE_FILE);

            return toReturn;
        }

        private static void RecordHistoricalPrice(string symbol, IMarketInfo marketInfo)
        {
            try
            {

                //we are going to save historical data to our own DB everytime we get data from  a provider
                HistoricalDataContainer db = new HistoricalDataContainer();

                //does the profile already exist in the DB?
                var matchingPortfolios = from curPortfolio in db.Portfolios
                                         where curPortfolio.Id == _portfolioId
                                         select curPortfolio;
                Portfolio myPortfolio = matchingPortfolios.FirstOrDefault();

                if (null != myPortfolio)
                {
                    //found it, load the stocks for access
                    myPortfolio.Stocks.Load();
                }
                else
                {
                    //create a new one
                    myPortfolio = new Portfolio() { Id = _portfolioId, CreationTimeStamp = DateTime.Now };
                    db.AddToPortfolios(myPortfolio);
                }

                Stock curStock = (from tmpStock in db.Stocks
                                  where tmpStock.Symbol.Equals(symbol, StringComparison.InvariantCultureIgnoreCase)
                                 select tmpStock).FirstOrDefault();
                if (null != curStock)
                {
                    //found it, load prices
                    curStock.Prices.Load();
                }
                else
                {
                    curStock = new Stock() { Id = Guid.NewGuid(), Symbol = marketInfo.Symbol };
                }


                if (myPortfolio.Stocks.Any(tmpStock => tmpStock.Symbol.Equals(curStock.Symbol, StringComparison.InvariantCultureIgnoreCase)))
                {
                    //already in there
                }
                else
                {//associate it w/current portfolio
                    myPortfolio.Stocks.Add(curStock);
                }

                //add the new price data if not already a price for that time
                if(!curStock.Prices.Any(tmpPrice => tmpPrice.TimeStamp == marketInfo.Timestamp))
                    curStock.Prices.Add(new Price() { Id = Guid.NewGuid(), Value = marketInfo.MarketPrice, TimeStamp = marketInfo.Timestamp });
                
                db.SaveChanges();
            }
            catch (Exception caught)
            {
                //for now ignore if we can't talk to the file
                //TODO: add exception handling that makes sense for this tool
            }
        }

        #endregion//Private Helper Methods
    }
}
