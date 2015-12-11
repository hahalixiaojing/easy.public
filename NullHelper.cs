using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public
{
    /// <summary>
    /// 空引用访问帮助
    /// </summary>
    public static class NullHelper
    {
        /// <summary>
        /// 如果指定值为null,则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static T IfNull<T>(T obj, T @default) where T : class
        {
            if (obj == null)
            {
                return @default;
            }
            return obj;
        }
        public static R IfNull<T, R>(T obj, R returnValue, Func<T, R> func) where T : class
        {
            if (obj == null)
            {
                return returnValue;
            }
            return func(obj);
        }
        /// <summary>
        /// 如果a为空或null则返回b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static String IfNullOrEmpty(String a, String b)
        {
            if (string.IsNullOrEmpty(a))
            {
                return a;
            }
            return b;
        }
    }
}
