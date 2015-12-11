using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Easy.Public.MyLog;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Newtonsoft.Json;
namespace Easy.Public.HttpRequestService
{
    public static class HttpWebRequestExtension
    {
        /*
         *  webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)"; 
         */

        /// <summary>
        /// JSON格式提交数据
        /// </summary>
        /// <typeparam name="T">数据对象</typeparam>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static HttpWebResponse Send<T>(this HttpWebRequest request, T data, JsonSerializerSettings settings = null)
        {
             string jsondata = "";
             if (settings != null)
             {
                 jsondata = JsonConvert.SerializeObject(data, settings);
             }
             else
             {
                 jsondata = JsonConvert.SerializeObject(data);
             }
             return Send(request, new StringBuilder(jsondata));
        }

        public static HttpWebResponse Send(this HttpWebRequest request, StringBuilder content)
        {
            if (content != null && content.Length > 0)
            {
                Byte[] bytes = Encoding.UTF8.GetBytes(content.ToString());
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
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
            }

            return request.GetResponse() as HttpWebResponse;
        }
        public static HttpWebResponse Send(this HttpWebRequest request)
        {
            return Send(request, null);
        }
        public static T GetBodyContent<T>(this HttpWebResponse response, Encoding encoding = null, bool close = true, JsonSerializerSettings settings = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            var result = GetBodyContent(response, encoding, close);

            if (settings == null)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            return JsonConvert.DeserializeObject<T>(result, settings);
        }
        public static String GetBodyContent(this HttpWebResponse response, Encoding encoding, bool close)
        {
            String result = String.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
            {
                try
                {
                    result = reader.ReadToEnd();
                }
                finally
                {
                    reader.Close();
                }
            }
            if (close)
            {
                response.Close();
            }

            return result;
        }
        public static String GetBodyContent(this HttpWebResponse response, bool close)
        {
            return GetBodyContent(response, Encoding.UTF8, close);
        }

    }

    public static class HttpRequestClient
    {
        public static HttpWebRequest Request(string url, string method, bool https)
        {
            return Request(url, method, "", https);
        }
        public static HttpWebRequest Request(string url, string method, string contentType = "", bool https = false)
        {
            if (https)
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                request.ContentType = contentType;
            }
            return request;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,SslPolicyErrors errors)
        {
            return true;  
        }  
    }
}
