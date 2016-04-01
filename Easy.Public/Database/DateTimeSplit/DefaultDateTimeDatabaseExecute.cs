using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public.Database.DateTimeSplit
{
    public class DefaultDateTimeDatabaseExecute : IDateTimeDatabaseExecute
    {
        private readonly DateTimeSplitDatabaseSelector selector;

        public DefaultDateTimeDatabaseExecute(DateTimeSplitDatabaseSelector selector)
        {
            this.selector = selector;
        }
        public void Add<ENTITY>(ENTITY entity, DateTime datetime, Action<IDateTimeSplitDatabase, ENTITY> execute)
        {
            var database = selector.Select(datetime);
            execute.Invoke(database, entity);
        }
        public Int64 Count(Func<IDateTimeSplitDatabase, Int64> execute)
        {
            var tasks = selector.All.Select(m =>
            {
                var t = new Task<Int64>(() =>
                {
                    return execute.Invoke(m);
                });
                t.Start();
                return t;
            });

            return Task.WhenAll(tasks).Result.Sum();
        }
        public Int64 Count(Query query, Func<IDateTimeSplitDatabase,Query, Int64> execute)
        {
            var databases = selector.Select(query.Start, query.End, OrderBy.DESC);
            var tasks = databases.Select(m =>
            {
                var t = new Task<Int64>(() =>
                {
                    return execute.Invoke(m, query);
                });
                t.Start();
                return t;
            });
            var result = Task.WhenAll(tasks);
            return result.Result.Sum();
        }
        public ENTITY FindBy<ENTITY, KEY>(KEY id, DateTime datetime, Func<IDateTimeSplitDatabase, KEY, ENTITY> execute)
        {
            return execute.Invoke(selector.Select(datetime), id);
        }

        public ENTITY FindBy<ENTITY, KEY>(KEY id, Func<IDateTimeSplitDatabase,KEY, ENTITY> execute)
        {
            var tasks = selector.All.Select(m =>
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
            return this.FindByIds<ENTITY, KEY>(ids, selector.All, execute);
        }

        public IEnumerable<ENTITY> FindByIds<ENTITY, KEY>(KEY[] ids, DateTime start, DateTime end, Func<IDateTimeSplitDatabase, KEY[], IEnumerable<ENTITY>> execute)
        {
            return this.FindByIds<ENTITY, KEY>(ids, selector.Select(start, end), execute);
        }

        public void Remove<KEY>(KEY id, Action<IDateTimeSplitDatabase, KEY> execute)
        {
            var tasks = selector.All.Select(m =>
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
            var database = selector.Select(datetime);
            execute.Invoke(database, id);
        }

        public void RemoveAll(Action<IDateTimeSplitDatabase> execute)
        {
            var tasks = selector.All.Select(m =>
             {
                 var t = new Task(() =>
                 {
                     execute.Invoke(m);
                 });
                 t.Start();
                 return t;
             });

            Task.WhenAll(tasks).Wait();
        }

        public DataTimeDataList<ENTITY> Select<ENTITY>(Query query,
            Func<IDateTimeSplitDatabase, Query,int, IEnumerable<ENTITY>> dataExecute,
            Func<IDateTimeSplitDatabase, Query, int> countExecute)
        {
            var databaseList = selector.Select(query.Start, query.End, query.OrderBy);

            var tasks = databaseList.Select(m =>
            {
                var task = new Task<int>(() =>
                {
                    return countExecute.Invoke(m, query);
                });
                task.Start();
                return task;
            });

            int[] databaseRows = Task.WhenAll(tasks).Result;
            int absoluteOffset = (query.PageIndex - 1) * query.PageSize;

            int endOffset = 0;
            int databaseIndex = -1;
            int relativeDatabaseOffset = 0;
            for (var i = 0; i < databaseRows.Length; i++)
            {
                int startOffset = endOffset;
                endOffset = endOffset + (databaseRows[i]);

                if (absoluteOffset >= startOffset && absoluteOffset < endOffset)
                {
                    databaseIndex = i;
                    relativeDatabaseOffset = absoluteOffset - startOffset;
                    break;
                }
            }

            if (databaseIndex == -1)
            {
                return new DataTimeDataList<ENTITY>(new ENTITY[0], databaseRows.Sum());
            }
            
            IDateTimeSplitDatabase database = databaseList.ToArray()[databaseIndex];

            List<ENTITY> rows = new List<ENTITY>();
            while (rows.Count < query.PageSize)
            {
                var thisDatabaseDataList = dataExecute.Invoke(database, query, relativeDatabaseOffset);
                rows.AddRange(thisDatabaseDataList);
                
                databaseIndex = databaseIndex + 1;
                if (databaseIndex >= databaseList.Count())
                {
                    break;
                }
                database = databaseList.ToArray()[databaseIndex];
            }

            var actualReturnRows = rows.Take(query.PageSize);
            return new DataTimeDataList<ENTITY>(rows, databaseRows.Sum());
        }

        public void Update<ENTITY>(ENTITY entity, Action<IDateTimeSplitDatabase> execute)
        {
            var tasks = selector.All.Select(m =>
            {
                var t = new Task(() =>
                {
                    execute.Invoke(m);
                });
                return t;
            });
            Task.WhenAll(tasks).Wait();
        }
        public void Update<ENTITY>(ENTITY entity, DateTime datetime, Action<IDateTimeSplitDatabase,ENTITY> execute)
        {
            var database = selector.Select(datetime);
            execute.Invoke(database, entity);
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
