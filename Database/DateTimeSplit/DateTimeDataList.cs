using System;
using System.Collections.Generic;

namespace Easy.Public.Database.DateTimeSplit
{
    public class DataTimeDataList<T>
    {
        public DataTimeDataList(IEnumerable<T> rows, long totalRows)
        {
            this.Rows = rows;
            this.TotalRows = totalRows;
        }
        public IEnumerable<T> Rows { get; private set; }
        public Int64 TotalRows { get; private set; }
    }
}
