using Easy.Public.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Easy.Public
{
    public static class EnumHelper
    {
        public static IDictionary<Int32, String> ToDictionary(Type type)
        {
            IDictionary<Int32, String> list = new Dictionary<Int32, String>();
            if (!type.IsEnum)
            {
                return list;
            }
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                Object[] attributes = field.GetCustomAttributes(false);
                string msg = "";
                if (attributes.Length == 0)
                {
                    msg = field.Name;
                }
                else
                {
                    DescriptionAttribute desc = field.GetCustomAttributes(false)[0] as DescriptionAttribute;
                    msg = desc.Message;
                }
                Int32 value = System.Convert.ToInt32(field.GetValue(null));
                list.Add(value,msg);
            }
            return list;
        }
    }
}

