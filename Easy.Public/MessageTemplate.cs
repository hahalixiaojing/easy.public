using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace Easy.Public
{
    /// <summary>
    /// <![CDATA[
    /// 文件模板
    /// <Template>
	///     <Text name="addOrder">
    ///		        测试模板【<PlaceHolder name="ShopName" />】测试模板<PlaceHolder name="PayUrl" />测试模板
    ///	    </Text>
    /// </Template>
    /// 
    /// 
    /// ]]>
    /// </summary>
    public class MessageTemplate
    {
        XPathNavigator navigator;
        public MessageTemplate(String file)
        {
            XPathDocument document = new XPathDocument(file);
            navigator = document.CreateNavigator();
        }

        public String GetMessage(string name, IList<PlaceHolder> placeHolders)
        {
            XPathNavigator node = navigator.SelectSingleNode(String.Format("/Template/Text[@name='{0}']", name));
            if (node == null)
            {
                return string.Empty;
            }
            var message = new StringBuilder();
            foreach (XPathNavigator subNode in node.SelectChildren(XPathNodeType.All))
            {
                if (subNode.NodeType == XPathNodeType.Text)
                {
                    message.Append(subNode.OuterXml);
                }
                else if (subNode.NodeType == XPathNodeType.Element)
                {
                    string text = ReplacePlaceHolder(subNode, placeHolders);

                    message.Append(text);
                }
            }
            return message.ToString();
        }
        private String ReplacePlaceHolder(XPathNavigator node, IList<PlaceHolder> placeHolders)
        {
            string name = node.GetAttribute("name", "");
            PlaceHolder holder = placeHolders.SingleOrDefault(p => p.Name == name);
            if (holder == null)
            {
                return string.Empty;
            }
            return holder.Value;
        }
    }

    public class PlaceHolder
    {
        public PlaceHolder(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public String Name
        {
            get;
            private set;
        }
        public String Value
        {
            get;
            private set;
        }
    }
}
