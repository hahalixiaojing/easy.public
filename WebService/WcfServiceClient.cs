using Easy.Public.HttpRequestService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Easy.Public.WebService
{
    public static class WcfServiceClient
    {
        public static T Request<T>(String url, string method,string soapaction , NamedParameter[] parameters,string @namespace = "http://tempuri.org/")
        {
            var request = HttpRequestClient.Request(url, "POST");
            request.ContentType = "text/xml;charset=utf-8";
            request.Headers.Add("SOAPAction:" + soapaction);

            StringBuilder requestData = GetRequestXml(method, @namespace, parameters);

            string rs = request.Send(requestData).GetBodyContent(true);

            return GetResponseObject<T>(method, @namespace, rs);
        }
        public static void Request(String url, string method, string soapaction, NamedParameter[] parameters, string @namespace = "http://tempuri.org/")
        {
            var request = HttpRequestClient.Request(url, "POST");
            request.ContentType = "text/xml;charset=utf-8";
            request.Headers.Add("SOAPAction:" + soapaction);

            StringBuilder requestData = GetRequestXml(method, @namespace, parameters);
            request.Send(requestData).GetBodyContent(true);
        }

        private static StringBuilder GetRequestXml(string method, string @namespace, NamedParameter[] parameter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.Append("<s:Body>");
            sb.AppendFormat("<{0} xmlns=\"{1}\">", method, @namespace);
            sb.Append(GetParametersXml(parameter));
            sb.AppendFormat("</{0}>",method);
            sb.Append("</s:Body>");
            sb.Append("</s:Envelope>");

            return sb;
        }
        private static String GetParametersXml(NamedParameter[] namedParams)
        {
            StringBuilder paramListXml = new StringBuilder();
            foreach (NamedParameter param in namedParams)
            {
                paramListXml.AppendFormat("<{0}>", param.Name);
                paramListXml.Append(GetParamXml(param));
                paramListXml.AppendFormat("</{0}>", param.Name);
            }
            return paramListXml.ToString();
        }
        private static String GetParamXml(NamedParameter param)
        {

            StringBuilder paramXml = new StringBuilder();
            using (TextWriter tw = new StringWriter(paramXml))
            {

                XmlSerializer x = new XmlSerializer(param.ValueType, new XmlRootAttribute(param.Name));
                x.Serialize(tw, param.Value);
            }
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(paramXml.ToString());

            return xmldoc.SelectSingleNode(param.Name).InnerXml;
        }
        private static V GetResponseObject<V>(String method,string @namespace, String responeXml)
        {
            V res = default(V);
            using (TextReader tReader = new StringReader(responeXml))
            {
                XPathDocument doc = new XPathDocument(tReader);
                XPathNavigator navi = doc.CreateNavigator();

                XmlNamespaceManager nsp = new XmlNamespaceManager(navi.NameTable);
                nsp.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                nsp.AddNamespace("u", @namespace);

                XPathNavigator resultNav = navi.SelectSingleNode(String.Format("s:Envelope/s:Body/u:{0}Response/u:{0}Result", method), nsp);

                if (resultNav != null)
                {
                    String outXml = resultNav.OuterXml;
                    outXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + outXml;
                    XmlRootAttribute attr = new XmlRootAttribute(String.Format("{0}Result", method));
                    attr.Namespace = @namespace;

                    XmlSerializer xmlSer = new XmlSerializer(typeof(V), attr);
                    using (TextReader reader = new StringReader(outXml))
                    {
                        res = (V)xmlSer.Deserialize(reader);
                    }
                }
            }
            return res;
        }
    

        //public static void Push(SearchEngineModel[] models)
        //{
        //    var request = HttpRequest.Request(ConfigurationManager.AppSettings["search"], "POST");
        //    request.ContentType = "text/xml;charset=utf-8";
        //    request.Headers.Add("SOAPAction:http://tempuri.org/IAddProductsService/AddProducts");
        //    string rs = request.Send(wcfdata).GetBodyContent(true);
        //}
        //private static StringBuilder GetWcfData(SearchEngineModel[] model)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
        //    sb.Append("<s:Body>");
        //    sb.Append("<AddProducts xmlns=\"http://tempuri.org/\">");
        //    sb.Append("<jsonProducts>");
        //    sb.Append(JsonConvert.SerializeObject(model));
        //    sb.Append("</jsonProducts>");
        //    sb.Append("</AddProducts>");
        //    sb.Append("</s:Body>");
        //    sb.Append("</s:Envelope>");

        //    return sb;
        //}
    }
}
