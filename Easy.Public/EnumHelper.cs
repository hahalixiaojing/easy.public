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
        public static string GetText(int[] keys, string[] text, int currentKeys, string defaultText)
        {
            int index = -1 ;
            for(var i = 0; i < keys.Length; i++)
            {
                if(currentKeys == keys[i])
                {
                    index = i;
                    break;
                }
            }
            if(index == -1)
            {
                return defaultText;
            }
            return text[index];
        }
    }
}

