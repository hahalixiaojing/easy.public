using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Easy.Public.MyLog
{
    public class PlainFileLogger : IMyLogger
    {
        #region IMyLog Members
        public void Initialization() { }
        public void WriteLog(Log log)
        {

            StringBuilder logText = new StringBuilder();
            logText.Append("tag : ");
            logText.AppendLine(log.Tag);
            logText.Append("message : ");
            logText.AppendFormat("{0}", log.Message);
            logText.Append("datetime : ");
            logText.AppendLine(log.DateTime.ToString());
            logText.AppendLine("=====================================================");

            String logFile = this.GetLogFileName(log.LogLevel);
            File.AppendAllText(logFile, logText.ToString(),Encoding.UTF8);
        }
        #endregion
        public String LogDir
        {
            get;
            set;
        }
        public String GetLogFileName(LogLevel level)
        {
            string subPath = string.Empty;
            if (String.IsNullOrEmpty(LogDir))
            {
                subPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            }
            else if (this.LogDir.Contains(':'))
            {
                subPath = this.LogDir;
            }
            else
            {
                subPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.LogDir);
            }
            if (!Directory.Exists(subPath))
            {
                Directory.CreateDirectory(subPath);
            }

            subPath = Path.Combine(subPath, level.ToString());
            if (!Directory.Exists(subPath))
            {
                Directory.CreateDirectory(subPath);
            }
            return Path.Combine(subPath, DateTime.Now.ToString("yyyyMMdd") + ".log");
        }
    }
}
