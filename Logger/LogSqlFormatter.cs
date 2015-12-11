using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Easy.Public.MyLog
{
    public class LogSqlFormatter : IFormatProvider, ICustomFormatter
    {
        #region IFormatProvider Members

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        #endregion

        #region ICustomFormatter Members

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            Log log = arg as Log;
            if (format == "t")
            {
                return log.Tag;
            }
            else if (format == "m")
            {
                return string.Format("{0}", log.Message);
            }
            else if (format == "d")
            {
                return log.DateTime.ToString();
            }
            else if (format == "l")
            {
                return log.LogLevel.ToString();
            }
            return string.Empty;
        }
        #endregion
    }
}
