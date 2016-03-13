using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Public.Database.DateTimeSplit;

namespace Easy.Public.Database
{
    public static class DatabaseExecute
    {
        private static IDateTimeDatabaseExecute defaultDateTimeDatabaseExecute = new DefaultDateTimeDatabaseExecute();

        /// <summary>
        /// 按时间分隔数据库
        /// </summary>
        public static IDateTimeDatabaseExecute DateTimeDatabaseExecute
        {
            get
            {
                return defaultDateTimeDatabaseExecute;
            }
            set { defaultDateTimeDatabaseExecute = value; }
        }
    }
}
