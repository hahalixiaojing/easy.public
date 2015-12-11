using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Easy.Public.MyLog
{
    public class ConsoleLogger : IMyLogger
    {
        #region IMyLog Members
        public void Initialization() { }
        public void WriteLog(Log log)
        {
            Debug.WriteLine(String.Format("tag : {0}", log.Tag));
            Debug.WriteLine(String.Format("message : {0}", log.Message));
            Debug.WriteLine(String.Format("datetime : {0}", log.DateTime));
            Debug.WriteLine(String.Format("type: {0}", log.LogLevel.ToString()));
            Debug.WriteLine("==========================================");
        }
        #endregion
    }
}
