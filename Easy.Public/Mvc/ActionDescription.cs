using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Easy.Public.Mvc
{
    public class ActionDescription
    {
        public String Alias { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public String ActionPath { get; set; }
        public String Description { get; set; }
        public Boolean IsAuthorization { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public Int32 HashCode
        {
            get
            {
                return this.GetHashCode();
            }
        }

        public override Int32 GetHashCode()
        {
            if (!String.IsNullOrEmpty(this.Alias))
            {
                return (ActionPath.ToUpperInvariant() + "_" + Alias.ToUpperInvariant()).GetHashCode();
            }
            return ActionPath.ToUpperInvariant().GetHashCode();
        }
    }
}
