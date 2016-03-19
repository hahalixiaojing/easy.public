using Easy.Public.Database.DateTimeSplit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public.Test.Database
{
    
    public class MemoryListDateTimeSplitDatabase : DefaultDateTimeSplitDatabase
    {
        List<Tuple<int, string, DateTime>> datalist = new List<Tuple<int, string, DateTime>>();

        public MemoryListDateTimeSplitDatabase(DateTime start, DateTime end, int databaesIndex)
            : base(start, end, databaesIndex)
        {

        }

        public override object Database
        {
            get
            {
                return datalist;
            }
            protected set
            {
                datalist = value as List<Tuple<int, string, DateTime>>;
            }
        }
    }
}
