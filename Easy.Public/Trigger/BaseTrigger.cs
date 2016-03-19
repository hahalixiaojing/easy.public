using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.Trigger
{
    public abstract class BaseTrigger : ITrigger
    {
        delegate void TriggerDelegate(Object obj, params Object[] extra);

        public BaseTrigger()
        {
            this.IsEnable = true;
        }

        public virtual string Name
        {
            get;
            set;
        }
        public virtual bool IsAsync
        {
            get;
            set;
        }

        public virtual bool IsEnable
        {
            get;
            set;
        }

        public abstract bool IsActivate(Object obj);
        public abstract void Trigger(Object obj, params Object[] extra);
        public void TriggerAsync(Object obj, params Object[] extra)
        {
            TriggerDelegate @delegate = Trigger;

            @delegate.BeginInvoke(obj, extra, null, null);
        }
    }
}
