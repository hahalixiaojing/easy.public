using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public.Database.DateTimeSplit
{
    public interface IDateTimeSplitDatabase
    {
        int Index
        {
            get;
        }

        DateTime Start
        {
            get;
        }

        DateTime End
        {
            get;
        }
        object Database { get; }

        bool IsSelected(DateTime start, DateTime end);
        bool IsSelected(DateTime datetime);
    }
}
