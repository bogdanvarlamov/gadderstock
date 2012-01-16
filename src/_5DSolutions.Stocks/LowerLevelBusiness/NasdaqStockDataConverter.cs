using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using HtmlAgilityPack;

namespace _5DSolutions.WindowsMobile.EasyStocks.LowerLevelBusiness
{
    public class NasdaqStockDataConverter: TypeConverter
    {
        #region Conversions

        private List<StockInfo> GetStockInfoListFromString(object value)
        {
            List<StockInfo> toReturn = new List<StockInfo>();

            try
            {
                XDocument rssDoc = XDocument.Parse(value as string);

                string cData = (from desc in rssDoc.Descendants("item").Elements("description")
                                where desc.Value != string.Empty
                                select desc.Value).First();

                cData = cData.Replace("<![CDATA", string.Empty).Replace("]]>", string.Empty);

                HtmlDocument quotesTables = new HtmlDocument();
                quotesTables.OptionFixNestedTags = true;
                quotesTables.OptionOutputAsXml = true;
                quotesTables.LoadHtml(cData);

                quotesTables.Save("test.xml");
                MemoryStream ms = new MemoryStream();
                quotesTables.Save(ms);
                ms.Position = 0;

                XmlReaderSettings xrs = new XmlReaderSettings();
                xrs.IgnoreWhitespace = true;

                XmlReader xr = XmlReader.Create(ms);
                //xr.MoveToContent();

                XDocument tablesDoc = XDocument.Load(xr);
            }
            catch (Exception caught)
            {
                //TODO: add some exception handling
            }

            return toReturn;
        }

        #endregion//Conversions
    }
}
