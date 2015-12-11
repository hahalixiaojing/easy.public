using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easy.Public.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Field )]
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(String desc)
        {
            this.Message = desc;
        }

        public String Message { get; private set; }
    }
}