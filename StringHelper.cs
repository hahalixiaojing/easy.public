using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public
{
    public static class StringHelper
    {
        public static String ToString(Object inputValue, String defaultValue, Boolean isTrim = true)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            if (isTrim)
            {
                return inputValue.ToString().Trim();
            }
            return inputValue.ToString();
        }
        public static Int32 ToInt32(Object inputValue, Int32 defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }

            Int32 convertResult;
            if (!Int32.TryParse(inputValue.ToString(), out convertResult))
            {
                return defaultValue;
            }
            return convertResult;
        }
        public static Int32? ToInt32(Object inputValue, Int32? defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            Int32 convertResult;
            if (!Int32.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return new Nullable<Int32>(convertResult);
        }
        public static Int64 ToInt64(Object inputValue, Int64 defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }

            Int64 convertResult;
            if (!Int64.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return convertResult;
        }
        public static Int64? ToInt64(Object inputValue, Int64? defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            Int64 convertResult;
            if (!Int64.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return new Nullable<Int64>(convertResult);
        }
        public static Int16 ToInt16(Object inputValue, Int16 defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }

            Int16 convertResult;
            if (!Int16.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return convertResult;
        }
        public static Int16? ToInt16(Object inputValue, Int16? defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            Int16 convertResult;
            if (!Int16.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return new Nullable<Int16>(convertResult);
        }
        public static Decimal ToDecimal(Object inputValue, Decimal defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }

            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }

            Decimal convertResult;
            if (!Decimal.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return convertResult;
        }
        public static Decimal? ToDecimal(Object inputValue, Decimal? defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }

            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            Decimal convertResult;
            if (!Decimal.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return new Nullable<Decimal>(convertResult);
        }
        public static Single ToSingle(Object inputValue, Single defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }

            Single convertResult;
            if (!Single.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return convertResult;
        }
        public static Single? ToSingle(Object inputValue, Single? defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            Single convertResult;
            if (!Single.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return new Nullable<Single>(convertResult);
        }
        public static Double ToDouble(Object inputValue, Double defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }

            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }

            Double convertResult;
            if (!Double.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return convertResult;
        }
        public static Double? ToDouble(Object inputValue, Double? defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }

            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            Double convertResult;
            if (!Double.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return new Nullable<Double>(convertResult);
        }
        public static Boolean ToBoolean(Object inputValue, Boolean defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            Boolean convertResult;
            if (!Boolean.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return convertResult;
        }
        public static Boolean? ToBoolean(Object inputValue, Boolean? defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            Boolean convertResult;
            if (!Boolean.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return new Nullable<Boolean>(convertResult);
        }
        public static DateTime ToDateTime(Object inputValue, DateTime defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            DateTime convertResult;
            if (!DateTime.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return convertResult;
        }
        public static DateTime? ToDateTime(Object inputValue, DateTime? defaultValue)
        {
            if (inputValue == null)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue.ToString()))
            {
                return defaultValue;
            }
            DateTime convertResult;
            if (!DateTime.TryParse(inputValue.ToString().Trim(), out convertResult))
            {
                return defaultValue;
            }
            return new Nullable<DateTime>(convertResult);
        }
        public static T ToEnum<T>(String inputValue, T defaultValue) 
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return defaultValue;
            }
            if (String.IsNullOrWhiteSpace(inputValue))
            {
                return defaultValue;
            }

            String[] names = Enum.GetNames(enumType);
            Int32 count = names.Count(n => n.ToUpper() == inputValue.Trim().ToUpper());
            if (count == 1)
            {
                return (T)Enum.Parse(enumType, inputValue.Trim(), true);
            }
            return defaultValue;
        }
        public static T ToEnum<T>(Int32 value, T defaultValue)
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return defaultValue;
            }

            Boolean isDefined = Enum.IsDefined(enumType, value);

            if (isDefined)
            {
                return (T)Enum.Parse(enumType, value.ToString());
            }
            return defaultValue;
        }
        /// <summary>
        /// 字节转成十六进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String BytesToHex(Byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 十六进制转字节
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static Byte[] HexToBytes(String hexString)
        {
            Int32 len = hexString.Length / 2;
            Byte[] ret = new Byte[len];
            for (Int32 i = 0; i < len; i++)
                ret[i] = (Byte)(Convert.ToInt32(hexString.Substring(i * 2, 2), 16));
            return ret;
        }
    }
}
