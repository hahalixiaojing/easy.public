using Easy.Public.HttpRequestService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public.MyLog
{
    public class RemoteLogger : IMyLogger
    {
        public string Url
        {
            get;
            set;
        }
        public void Initialization()
        {
            
        }

        public void WriteLog(Log log)
        {
            try
            {
                var req = HttpRequestClient.Request(this.Url, "POST", false);
                req.ContentType = "application/json";
                req.Send(new StringBuilder(JsonConvert.SerializeObject(log)));
            }
            catch { }
        }
    }
}
