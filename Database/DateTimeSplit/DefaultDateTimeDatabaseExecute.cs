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
            Func<IDateTimeSplitDatabase, Query, DataTimeDataList<ENTITY>> dataExecute,
            Func<IDateTimeSplitDatabase, Query, Int64> countExecute)
        {
            var databaseList = DateTimeSplitDatabaseManager.Instance.Select(query.Start, query.End, query.OrderBy);
            var startDatabase = query.DatabaseIndex == 0 ? databaseList.First() : DateTimeSplitDatabaseManager.Instance[query.DatabaseIndex];


            var tasks = databaseList.Select(m =>
            {
                var task = new Task<Int64>(() =>
                {
                    return countExecute.Invoke(m, query);
                });
                task.Start();
                return task;
            });
            long totalRows = Task.WhenAll(tasks).Result.Sum();

            //Task.Factory.

            throw new NotImplementedException();

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
