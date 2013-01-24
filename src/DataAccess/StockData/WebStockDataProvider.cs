using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace _5DSolutions.Stocks.DataAccess
{
    public class WebStockDataProvider: IStringStockDataProvider
    {

        #region Private Variables

        private ICredentials _dataCredentials;

        #endregion//Private Variables

        #region Constructors

        public WebStockDataProvider()
        {
            _dataCredentials = CredentialCache.DefaultCredentials;
        }

        public WebStockDataProvider(ICredentials dataCredentials)
            : this()
        {
            _dataCredentials = dataCredentials;
        }

        #endregion//Constructors

        #region IStockDataProvider Members

        public string GetLatestRawStockData(string uriToGetRawDataFrom)
        {
            string toReturn = string.Empty;

            try
            {

                if (CheckDataResourceAvailable(uriToGetRawDataFrom))
                {//the resourse seems to be there, go ahead and get it
                    WebRequest request = WebRequest.Create(uriToGetRawDataFrom);
                    request.Credentials = _dataCredentials;

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {//time to convert the stream into raw string data
                            using (StreamReader sr = new StreamReader(dataStream, GetEncodingToUse(response)))
                            {
                                toReturn = sr.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    //TODO: add some way to indicate that the URI is not valid
                }

            }
            catch
            {
                //TODO: add exception handling
            }

            return toReturn;
        }

        private static Encoding GetEncodingToUse(HttpWebResponse response)
        {
            Encoding toReturn = Encoding.Default;//use default (usually windows-1251)

            if (string.Empty != response.ContentEncoding)
            {
                toReturn = Encoding.GetEncoding(response.ContentEncoding);
            }
            else
            {
                //no content encoding
                if (string.Empty != response.CharacterSet)
                {
                    toReturn = Encoding.GetEncoding(response.CharacterSet);
                }
                else
                {
                    //dont know the encoding, use default
                }
            }

            return toReturn;
        }


        private bool CheckDataResourceAvailable(string uriToCheck)
        {
            bool toReturn = false;//assume not available

            try
            {

                WebRequest request = WebRequest.Create(uriToCheck);
                request.Credentials = _dataCredentials;

                request.Timeout = 10000; //set a 10second timeout, (exception will be thrown if this timeout is not enough)
                request.Method = "HEAD";
                
                using(WebResponse response = request.GetResponse())
                {
                    //indicate whether there is any content at this URI
                    toReturn = (string.Empty != response.ContentType);
                }
            }
            catch
            {//there was an exception, either timed out or the URI is invalid
                toReturn = false;
            }

            return toReturn;
        }

        #endregion
    }
}
