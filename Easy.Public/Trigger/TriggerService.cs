using Castle.DynamicProxy;
using Easy.Public.MyLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.Trigger
{
    public abstract class TriggerService : IInterceptor
    {
        protected abstract string Name { get; }
        protected abstract Object ArgsData(object[] args);

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();

                ITriggerContainer container = TriggerContainerFactory.GetContainer();
                IList<ITrigger> triggers = container.GetTriggers(this.Name);

                foreach (var trigger in triggers)
                {
                    try
                    {
                        Object data = this.ArgsData(invocation.Arguments);
                        if (trigger.IsEnable && trigger.IsActivate(data))
                        {
                            if (trigger.IsAsync)
                            {
                                trigger.TriggerAsync(data, invocation.Method.Name);
                            }
                            else
                            {
                                trigger.Trigger(data, invocation.Method.Name);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogManager.Error(trigger.Name, e.Message + e.StackTrace);
                    }
                }

            }
            catch (Exception e)
            {
                LogManager.Error(e.Message, e.StackTrace);
            }
        }
    }
}
