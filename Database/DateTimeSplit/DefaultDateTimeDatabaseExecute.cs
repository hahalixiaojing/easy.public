using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public.Database.DateTimeSplit
{
    public class DefaultDateTimeDatabaseExecute : IDateTimeDatabaseExecute
    {
        public void Add<ENTITY>(ENTITY entity, Action<IDateTimeSplitDatabase, ENTITY> execute)
        {
            execute.Invoke(DateTimeSplitDatabaseManager.Instance.Latest, entity);
        }
        public void Add<ENTITY>(ENTITY entity, DateTime datetime, Action<IDateTimeSplitDatabase, ENTITY> excute)
        {
            excute.Invoke(DateTimeSplitDatabaseManager.Instance.Select(datetime), entity);
        }

        public T Scalar<T>() where T : struct
        {
            throw new NotImplementedException();
        }

        public T Scalar<T>(Query query) where T : struct
        {
            throw new NotImplementedException();
        }

        public ENTITY FindBy<ENTITY, KEY>(KEY id, DateTime datetime, Func<IDateTimeSplitDatabase, KEY, ENTITY> execute)
        {
            return execute.Invoke(DateTimeSplitDatabaseManager.Instance.Select(datetime), id);
        }

        public ENTITY FindBy<ENTITY, KEY>(KEY id, Func<IDateTimeSplitDatabase,KEY, ENTITY> execute)
        {
            var tasks = DateTimeSplitDatabaseManager.Instance.All.Select(m =>
             {
                 var t = new Task<ENTITY>(() =>
                 {
                     return execute.Invoke(m, id);
                 });
                 t.Start();
                 return t;
             });

            Task<ENTITY[]> results = Task.WhenAll(tasks);
            results.Wait();
            return results.Result.SingleOrDefault(m => m != null);
        }

        public IEnumerable<ENTITY> FindByIds<ENTITY, KEY>(KEY[] ids, Func<IDateTimeSplitDatabase, KEY[], IEnumerable<ENTITY>> execute)
        {
            return this.FindByIds<ENTITY, KEY>(ids, DateTimeSplitDatabaseManager.Instance.All, execute);
        }


        public IEnumerable<ENTITY> FindByIds<ENTITY, KEY>(KEY[] ids, DateTime start, DateTime end, Func<IDateTimeSplitDatabase, KEY[], IEnumerable<ENTITY>> execute)
        {
            return this.FindByIds<ENTITY, KEY>(ids, DateTimeSplitDatabaseManager.Instance.Select(start, end), execute);
        }

        public void Remove<KEY>(KEY id, Action<IDateTimeSplitDatabase, KEY> execute)
        {
            var tasks = DateTimeSplitDatabaseManager.Instance.All.Select(m =>
            {
                var t = new Task(() =>
                {
                    execute.Invoke(m, id);
                });
                t.Start();
                return t;
            });

            Task.WhenAll(tasks).Wait();
        }

        public void Remove<KEY>(KEY id, DateTime datetime,Action<IDateTimeSplitDatabase,KEY> execute)
        {
            var database = DateTimeSplitDatabaseManager.Instance.Select(datetime);
            execute.Invoke(database, id);
        }

        public void RemoveAll(Action<IDateTimeSplitDatabase> execute)
        {
            var tasks = DateTimeSplitDatabaseManager.Instance.All.Select(m =>
             {
                 var t = new Task(() =>
                 {
                     execute.Invoke(m);
                 });
                 return t;
             });

            Task.WhenAll(tasks).Wait();
        }

        public DataTimeDataList<ENTITY> Select<ENTITY>(Query query,
            Func<IDateTimeSplitDatabase, Query,long, IEnumerable<ENTITY>> dataExecute,
            Func<IDateTimeSplitDatabase, Query, Int64> countExecute)
        {
            var databaseList = DateTimeSplitDatabaseManager.Instance.Select(query.Start, query.End, query.OrderBy);

            var tasks = databaseList.Select(m =>
            {
                var task = new Task<Int64>(() =>
                {
                    return countExecute.Invoke(m, query);
                });
                task.Start();
                return task;
            });

            long[] databaseRows = Task.WhenAll(tasks).Result;
            long absoluteOffset = (query.PageIndex - 1) * query.PageSize;

            long endOffset = 0;
            int databaseIndex = 0;
            long relativeDatabaseOffset = 0;
            for (var i = 0; i < databaseRows.Length; i++)
            {
                long startOffset = endOffset;
                endOffset = endOffset + databaseRows[i];

                if (absoluteOffset >= startOffset && absoluteOffset <= endOffset)
                {
                    databaseIndex = i;
                    relativeDatabaseOffset = absoluteOffset - startOffset;
                    break;
                }
            }
            //TODO:需要找到当前数据库具体的偏移位置
            IDateTimeSplitDatabase database = databaseList.ToArray()[databaseIndex];

            List<ENTITY> rows = new List<ENTITY>();
            while (rows.Count < query.PageSize)
            {
                var thisDatabaseDataList = dataExecute.Invoke(database, query, relativeDatabaseOffset);
                rows.AddRange(thisDatabaseDataList);

                if (databaseIndex + 1 > databaseList.Count())
                {
                    break;
                }
                database = databaseList.ToArray()[databaseIndex + 1];
               
            }

            var actualReturnRows = rows.Take(query.PageSize);
            return new DataTimeDataList<ENTITY>(rows, databaseRows.Sum());
        }

        public void Update<ENTITY>(ENTITY entity)
        {
            throw new NotImplementedException();
        }

        public void Update<ENTITY>(ENTITY entity, DateTime datetime)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ENTITY> FindByIds<ENTITY, KEY>(KEY[] ids, IEnumerable<IDateTimeSplitDatabase> databases, Func<IDateTimeSplitDatabase, KEY[], IEnumerable<ENTITY>> execute)
        {
            var tasks = databases.Select(m =>
            {
                var t = new Task<IEnumerable<ENTITY>>(() => {
                    return execute.Invoke(m, ids);

                });
                t.Start();
                return t;
            });

            Task<IEnumerable<ENTITY>[]> results = Task.WhenAll(tasks);
            results.Wait();

            List<ENTITY> dataList = new List<ENTITY>();
            foreach (var item in results.Result)
            {
                if (item.Count() > 0)
                {
                    dataList.AddRange(item);
                }
            }
            return dataList;
        }
    }
}
