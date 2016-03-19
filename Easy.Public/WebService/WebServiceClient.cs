using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Net;
using System.Xml.XPath;
namespace Easy.Public.WebService
{
    public class WebServiceClient
    {

        public WebServiceClient(String url,String @namespace)
        {
            this.Url = url;
            this.Namespace = @namespace;
        }
        public String Url { get; set; }
        public String @Namespace { get; set; }

        public V InvokeMethod<V>(String method, params NamedParameter[] namedParams)
        {
            String requestXml = this.GetRequestXml(method, namedParams);
            String responseXml = this.Request(requestXml, method);

            return this.GetResponseObject<V>(method, responseXml);
        }
        public V InvokeMethod<V>(String method)
        {
            String requestXml = this.GetRequestXml(method, null);
            String responseXml = this.Request(requestXml, method);

            return this.GetResponseObject<V>(method, responseXml);
        }

        private V GetResponseObject<V>(String method,String responeXml)
        {
            V res = default(V);
            using (TextReader tReader = new StringReader(responeXml))
            {
                XPathDocument doc = new XPathDocument(tReader);
                XPathNavigator navi = doc.CreateNavigator();

                XmlNamespaceManager nsp = new XmlNamespaceManager(navi.NameTable);
                nsp.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsp.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                nsp.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
                nsp.AddNamespace("u", this.Namespace);

                XPathNavigator resultNav = navi.SelectSingleNode(String.Format("soap:Envelope/soap:Body/u:{0}Response/u:{0}Result", method), nsp);

                if (resultNav != null)
                {
                    String outXml = resultNav.OuterXml;
                    outXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + outXml;
                    XmlRootAttribute attr = new XmlRootAttribute(String.Format("{0}Result", method));
                    attr.Namespace = this.Namespace;

                    XmlSerializer xmlSer = new XmlSerializer(typeof(V), attr);
                    using (TextReader reader = new StringReader(outXml))
                    {
                        res = (V)xmlSer.Deserialize(reader);
                    }
                }
            }
            return res;
        }

        private String GetRequestXml(String method,NamedParameter[] namedParams)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            stringBuilder.Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            stringBuilder.Append("<soap:Body>");
            stringBuilder.AppendFormat("<{0} xmlns=\"{1}\">", method, this.Namespace);

            if (namedParams != null && namedParams.Length > 0)
            {
                stringBuilder.Append(this.GetParametersXml(namedParams));
            }
            stringBuilder.AppendFormat("</{0}>", method);
            stringBuilder.Append("</soap:Body>");
            stringBuilder.Append("</soap:Envelope>");

            return stringBuilder.ToString();

        }

        private String GetParametersXml(NamedParameter[] namedParams)
        {
            StringBuilder paramListXml = new StringBuilder();
            foreach (NamedParameter param in namedParams)
            {
                paramListXml.AppendFormat("<{0}>", param.Name);
                paramListXml.Append(this.GetParamXml(param));
                paramListXml.AppendFormat("</{0}>", param.Name);
            }
            return paramListXml.ToString();
        }
        private String GetParamXml(NamedParameter param)
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

        private String Request(String webServiceXml,String method)
        {
            String resXml = String.Empty;

            WebRequest webRequest = WebRequest.Create(this.Url);
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml; charset=utf-8";
            webRequest.Headers.Add("SOAPAction", String.Format(this.Namespace + "{0}", method));

            Byte[] bytes = Encoding.UTF8.GetBytes(webServiceXml);
            webRequest.ContentLength = bytes.Length;

            using (Stream stream = webRequest.GetRequestStream())
            {
                try
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                finally
                {
                    stream.Close();
                }
            }
            try
            {
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        try
                        {
                            resXml = reader.ReadToEnd();
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                    webResponse.Close();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    ex.Response.Close();
                }
                throw new WebServiceException(ex.Message);
            }
            catch
            {
                throw;
            }
            return resXml;
        }
    }
}
