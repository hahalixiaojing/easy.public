using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Easy.Public.Trigger
{
    public static class TriggerContainerFactory
    {
        private static ITriggerContainer container;
        private static ITriggerContainer emptyContainer = new EmptyTriggerContainer();

        static TriggerContainerFactory()
        {
            string customerContainer = ConfigurationManager.AppSettings["trigger_container"];
            if (string.IsNullOrEmpty(customerContainer))
            {
                container = new DefaultTriggerContainer();
            }
            else
            {
                Type type = Type.GetType(customerContainer);
                if (type == null)
                {
                    throw new TypeLoadException("can not find type " + customerContainer);
                }
                container = Activator.CreateInstance(type) as ITriggerContainer;
                if (container == null)
                {
                    throw new NotImplementedException("not implement interface ITriggerContainer");
                }
            }

            container.Init();
        }

        public static ITriggerContainer GetContainer()
        {
            return container ?? emptyContainer;
        }
    }
}
