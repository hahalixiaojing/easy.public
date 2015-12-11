using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.MyLog
{
    public class Log
    {
        public Log(String tag, Object message, LogLevel logLevel)
        {
            this.Tag = tag;
            this.Message = message;
            this.LogLevel = logLevel;
            this.DateTime = DateTime.Now;
        }
        public String Tag
        {
            get;
            private set;
        }
        public Object Message
        {
            get;
            private set;
        }
        public DateTime DateTime
        {
            get;
            private set;
        }
        public LogLevel LogLevel
        {
            get;
            private set;
        }
    }

  
}
