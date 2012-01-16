using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _5DSolutions.Stocks.Business
{

    public class YahooCSVConverter<T> : StringToMarketInfoConverter<T>, IStringToMarketInfoConverter<T> where T : IMarketInfo, new()
    {
        public override IEnumerable<T> GetStockInfoListFromString(object stringObject)
        {
            List<T> toReturn = new List<T>();

            if (null != stringObject
            && !string.Empty.Equals((string)stringObject))
            {

                string[] csvValues = ((string)stringObject).Split(',');

                if (csvValues.Length >= 4)
                {
                    for (int index = 0; index < csvValues.Length; index++)
                    {//get rid of the quotes!
                        csvValues[index] = csvValues[index].Replace("\"", string.Empty);
                    }

                    decimal price = 0.0M;
                    decimal.TryParse(csvValues[1], out price);

                    DateTime time = new DateTime();
                    DateTime.TryParse(string.Format("{0} {1}", csvValues[2], csvValues[3]), out time);

                    T tmp = new T();
                    ((IMarketInfo)tmp).Symbol = csvValues[0];
                    ((IMarketInfo)tmp).MarketPrice = price;
                    ((IMarketInfo)tmp).Timestamp = time;

                    toReturn.Add(tmp);
                }
                else
                {
                    //wrong formatting...
                }
            }
            else
            {
                //got wrong data
            }
            return toReturn as IEnumerable<T>;
        }

    }
}
