using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace _5DSolutions.Stocks.LowerLevelBusiness.DataConverters
{
    /// <summary>
    /// this class just exposes the common conversion checks for all string data converters
    /// </summary>
    public abstract class StringToSimpleStockInfoCoverter<T>: TypeConverter, IStringToSimpleStockInfoConverter<T> where T:ISimpleStockInfo, new()
    {

        #region Conversion Checking
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            bool toReturn = false; //assume we can't convert

            if (sourceType.Equals(typeof(string)))
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

            if (destinationType.Equals(typeof(List<ISimpleStockInfo>)))
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

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            object toReturn = null;


            //make sure we can convert from the object requesting it
            if (this.CanConvertFrom(value.GetType())
             && this.CanConvertTo(destinationType))
            {

                if (destinationType.Equals(typeof(List<T>))
                 && value is string)
                {
                    toReturn = GetStockInfoListFromString(value);
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

        #region IStringToStockInfoConverter Members

        public virtual List<T> GetStockInfoListFromString(object stringObject)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
