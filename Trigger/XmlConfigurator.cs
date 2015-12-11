using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Easy.Public.Trigger
{
    public static class XmlConfigurator
    {
        public static void Configure(FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(fileInfo.FullName);
            }
            XPathDocument doc = new XPathDocument(fileInfo.FullName);
            XPathNavigator navigator = doc.CreateNavigator();

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(navigator.NameTable); //namespace   
            namespaceManager.AddNamespace("abc", "http://www.39541240.com/triggers");
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            XPathNodeIterator it = navigator.Select("abc:triggers/abc:group", namespaceManager);
            foreach (XPathNavigator navi in it)
            {
                string name = navi.GetAttribute("name", "");
                var triggers = new List<ITrigger>();
                XPathNodeIterator it_triggers = navi.Select("abc:trigger", namespaceManager);
                foreach (XPathNavigator item in it_triggers)
                {
                    bool is_enable = Convert.ToBoolean(item.GetAttribute("is_enable", ""));
                    bool is_async = Convert.ToBoolean(item.GetAttribute("is_async", ""));
                    string trigger_name = item.GetAttribute("name","");
                    string typeString = item.GetAttribute("type", "");

                    Type type  = Type.GetType(typeString);
                    if (type == null)
                    {
                        throw new TypeLoadException(typeString);
                    }
                    ITrigger trigger = Activator.CreateInstance(type) as ITrigger;
                    if (trigger == null)
                    {
                        throw new NotImplementedException("ITrigger");
                    }
                    trigger.IsEnable = is_enable;
                    trigger.IsAsync = is_async;
                    trigger.Name = trigger_name;

                    triggers.Add(trigger);
                }
                ITriggerContainer container = TriggerContainerFactory.GetContainer();
                container.Add(name, triggers);
            }
        }
    }
}
