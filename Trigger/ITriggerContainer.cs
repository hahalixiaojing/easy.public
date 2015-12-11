using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.Trigger
{
    public interface ITriggerContainer
    {
        void Init();
        void Add(String name, IList<ITrigger> triggers);
        IList<ITrigger> GetTriggers(string triggerName);

    }
}
