using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.Mvc
{
    public class ControllerDescription
    {
        public String Name { get; set; }
        public String UrlTemplate { get; set; }
        public String ClassPath { get; set; }
        public Type ControllerType { get; set; }
        public IList<ActionDescription> ActionDescription { get; set; }
    }
}
