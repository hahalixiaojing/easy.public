using System;

namespace Easy.Public.WebService
{
    public class NamedParameter
    {
        public NamedParameter(String name,Object value,Type valueType)
        {
            this.Name = name;
            this.Value = value;
            this.ValueType = valueType;
        }

        public String Name { get; private set; }
        public Object Value { get; private set;}
        public Type ValueType { get; private set; }
    }
}
