using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.MvcSecurity
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ResourceName : Attribute
    {
        public ResourceName(String name)
        {
            this.Name = name;
        }

        public String Name { get; private set; }
    }
}
