using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.MyLog
{
    public interface IMyLogger
    {
        void Initialization();
        void WriteLog(Log log);
    }
}
