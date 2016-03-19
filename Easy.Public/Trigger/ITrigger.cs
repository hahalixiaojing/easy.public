using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.Trigger
{
    public interface ITrigger 
    {
        String Name { get; set; }
        Boolean IsAsync { get; set; }
        Boolean IsEnable { get; set; }
        Boolean IsActivate(Object obj);
        void Trigger(Object obj, params Object[] extra);
        void TriggerAsync(Object obj, params Object[] extra);
    }

}
