using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.Trigger
{
    public class DefaultTriggerContainer : ITriggerContainer
    {
        private IDictionary<String, IList<ITrigger>> _triggers = new Dictionary<String, IList<ITrigger>>();
        public void Init() { }
        public void Add(string name, IList<ITrigger> triggers)
        {
            if (_triggers.ContainsKey(name))
            {
                _triggers[name] = triggers;
            }
            else
            {
                _triggers.Add(name, triggers);
            }
        }
        public IList<ITrigger> GetTriggers(string group)
        {
            if (_triggers.ContainsKey(group))
            {
                return _triggers[group] ?? new List<ITrigger>(0);
            }
            return new List<ITrigger>();
        }
    }

    class EmptyTriggerContainer : ITriggerContainer
    {

        public void Init()
        {
        }

        public IList<ITrigger> GetTriggers(string triggerName)
        {
            return new List<ITrigger>(0);
        }


        public void Add(string name, IList<ITrigger> triggers)
        {
        }
    }
}
