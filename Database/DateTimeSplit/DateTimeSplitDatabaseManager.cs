using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Public.Database.DateTimeSplit
{
    public class DateTimeSplitDatabaseManager
    {
        private static object lockObj = new object();
        private static readonly IList<IDateTimeSplitDatabase> _DATABASE = new List<IDateTimeSplitDatabase>();
        private static DateTimeSplitDatabaseManager _instance;

        private DateTimeSplitDatabaseManager()
        {

        }

        public static DateTimeSplitDatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new DateTimeSplitDatabaseManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Register(IDateTimeSplitDatabase database)
        {
            _DATABASE.Add(database);
        }
        /// <summary>
        /// 根据时间范围选择合适的数据库
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<IDateTimeSplitDatabase> Select(DateTime start,DateTime end)
        {
            return _DATABASE.Where(m => m.IsSelected(start, end));
        }
        /// <summary>
        /// 根据时间点选择合适的数据库
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public IDateTimeSplitDatabase Select(DateTime datetime)
        {
            return _DATABASE.Where(m => m.IsSelected(datetime.Date)).FirstOrDefault();
        }
        /// <summary>
        /// 获得当前数据库的下一个数据库（时间更远的库）
        /// </summary>
        /// <param name="currentDatabaseIndex"></param>
        /// <returns></returns>
        public IDateTimeSplitDatabase Next(int currentDatabaseIndex)
        {
            return _DATABASE.SingleOrDefault(m => m.Index == (currentDatabaseIndex - 1));
        }
        public IDateTimeSplitDatabase Previous(int currentDatabaseIndex)
        {
            return _DATABASE.SingleOrDefault(m => m.Index == (currentDatabaseIndex + 1));
        }
        /// <summary>
        /// 第一个库（时间最近的库）
        /// </summary>
        public IDateTimeSplitDatabase First
        {
            get
            {
                return _DATABASE.SingleOrDefault(m => m.Index == _DATABASE.Max(x => x.Index));
            }
        }
        /// <summary>
        /// 最后一个库（时间最远的库）
        /// </summary>
        public IDateTimeSplitDatabase Last
        {
            get
            {
                return _DATABASE.SingleOrDefault(m => m.Index == _DATABASE.Min(x => x.Index));
            }
        }

        /// <summary>
        /// 全部数据库
        /// </summary>
        public IEnumerable<IDateTimeSplitDatabase> All
        {
            get
            {
                return _DATABASE.OrderByDescending(m => m.Index);
            }
        }

    }
}
