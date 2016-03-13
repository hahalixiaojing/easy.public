using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public.Database.DateTimeSplit
{
    public class DataTimeDataList<T>
    {
        public DataTimeDataList(IEnumerable<T> rows,int totalRows, int innerPageInex)
        {
            this.Rows = rows;
            this.TotalRows = totalRows;
            this.InnerPageIndex = innerPageInex;
        }

        public IEnumerable<T> Rows { get; private set; }
        public int TotalRows { get; private set; }
        public int InnerPageIndex { get; private set; }
    }

}
