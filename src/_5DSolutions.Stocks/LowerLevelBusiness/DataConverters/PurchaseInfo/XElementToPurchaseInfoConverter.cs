using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace _5DSolutions.Stocks.LowerLevelBusiness.DataConverters.PurchaseInfo
{
    public class XElementToPurchaseInfoConverter<T>: TypeConverter where T:IPurchaseInfo, new()
    {
        #region Conversion Checking
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            bool toReturn = false; //assume we can't convert

            if (sourceType.Equals(typeof(XElement)))
            {
                toReturn = true;
            }
            else if (sourceType.Equals(typeof(T)))
            {
                toReturn = true;
            }
            else
            {
                toReturn = base.CanConvertFrom(context, sourceType);
            }

            return toReturn;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            bool toReturn = false;//assume we can't convert

            if (destinationType.Equals(typeof(IPurchaseInfo)))
            {
                toReturn = true;
            }
            else
            {
                toReturn = base.CanConvertTo(context, destinationType);
            }

            return toReturn;
        }
        #endregion// Conversion Checking



        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            
            object toReturn = null;


            //make sure we can convert from the object requesting it
            if (this.CanConvertFrom(value.GetType()))
            {
                if (value is T)
                {
                    toReturn = GetXElementFromPurchaseInfo((T)value);
                }
                else
                {
                    throw new Exception("Cannot perform conversion between these types, check the CanConvertTo and CanConvertFrom methods before attempting");
                }

            }
            return toReturn;
        }

        private XElement GetXElementFromPurchaseInfo(T purchaseInfo)
        {
            XElement toReturn = null;

            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                using (XmlWriter xw = XmlWriter.Create(ms))
                {
                    XmlWriterSettings tmp = new XmlWriterSettings();
                    tmp.OmitXmlDeclaration = true;
                    tmp.ConformanceLevel = ConformanceLevel.Fragment;

                    xs.Serialize(xw, purchaseInfo);
                }

                ms.Position = 0;//rewind the stream

                toReturn = XElement.Load(XmlReader.Create(ms));
            }

            return toReturn;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            object toReturn = null;


            //make sure we can convert from the object requesting it
            if (this.CanConvertFrom(value.GetType())
             && this.CanConvertTo(destinationType))
            {

                if (destinationType.Equals(typeof(IPurchaseInfo))
                 && value is XElement)
                {
                    toReturn = GetPuchaseInfoFromXElement((XElement)value);
                }
                else
                {
                    base.ConvertTo(context, culture, value, destinationType);
                }
            }
            else
            {
                throw new Exception("Cannot perform conversion between these types, check the CanConvertTo and CanConvertFrom methods before attempting");
            }

            return toReturn;
        }

        private IPurchaseInfo GetPuchaseInfoFromXElement(XElement xElement)
        {
            IPurchaseInfo toReturn = null;

            XmlSerializer xs = new XmlSerializer(typeof(T));
            
            using(System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                using (XmlWriter xw = XmlWriter.Create(ms))
                {
                    XmlWriterSettings tmp = new XmlWriterSettings();
                    tmp.OmitXmlDeclaration = true;
                    tmp.ConformanceLevel = ConformanceLevel.Fragment;

                    xElement.Save(xw);//save it to the writer, and underlying stream
                }

                ms.Position = 0;//rewind the stream

                toReturn = (T)xs.Deserialize(ms);
            }
            return toReturn;
        }

    }
}
