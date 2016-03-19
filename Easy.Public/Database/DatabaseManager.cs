using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Public.Database.DateTimeSplit;

namespace Easy.Public.Database
{
    public static class DatabaseManager
    {
        private static readonly IDictionary<string, IDateTimeDatabaseExecute> dataTimeDatabaseExecute = new Dictionary<string, IDateTimeDatabaseExecute>();

        public static void RegisterDataTimeDatabaseExecute(string name, IDateTimeDatabaseExecute execute)
        {
            dataTimeDatabaseExecute.Add(name, execute);
        }

        public static IDateTimeDatabaseExecute DataTimeDatabaseExecute(string name)
        {
            return dataTimeDatabaseExecute[name];
        }
            
    }
}
