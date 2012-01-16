using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace _5DSolutions.Stocks.Business.AppState
{
    /// <summary>
    /// handles the saving/loading of the application state
    /// </summary>
    public static class ApplicationStateManager
    {
        public static void SaveCurrentAppState(ApplicationState curState, string relativePathToAppStateDoc)
        {
            WriteState(curState, GetUriToAppStateDoc(relativePathToAppStateDoc));
        }

        public static ApplicationState GetLastAppState(string relativePathToAppStateDoc)
        {
            ApplicationState toReturn = null;
            Uri uri = GetUriToAppStateDoc(relativePathToAppStateDoc);
            XDocument xmlAppStateDoc = null;

            if (System.IO.File.Exists(uri.LocalPath))
            {//load it

                //TODO: add some schema validation for the document to make sure its properly formatted, etc.
                xmlAppStateDoc = XDocument.Load(uri.LocalPath);

                //serialize the xml to the appstate to return
                XmlSerializer xs = new XmlSerializer(typeof(ApplicationState));
                toReturn = xs.Deserialize(xmlAppStateDoc.CreateReader()) as ApplicationState;
            }
            else
            {//first time, lets create it...
                if (System.IO.Directory.Exists(uri.LocalPath))
                {
                    //already there, the file is just gone...
                }
                else
                {//make the dir
                    System.IO.Directory.CreateDirectory(uri.LocalPath.Replace(uri.Segments.Last(), string.Empty));
                }

                //create a new appstate
                toReturn = new ApplicationState(new List<DisplayedStock>());

                WriteState(toReturn, uri);

            }

            return toReturn;
        }

        private static Uri GetUriToAppStateDoc(string relativePathToAppStateDoc)
        {
            Uri uri = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + relativePathToAppStateDoc);
            return uri;
        }

        private static void WriteState(ApplicationState toReturn, Uri uri)
        {
            using (System.IO.FileStream tmp = System.IO.File.Create(uri.LocalPath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ApplicationState));
                xs.Serialize(tmp, toReturn);//serialize it and save it
            }
        }
    }
}
