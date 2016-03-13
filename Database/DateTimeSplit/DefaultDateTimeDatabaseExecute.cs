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
            execute.Invoke(DateTimeSplitDatabaseManager.Instance.First, entity);
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
            var tasks = DateTimeSplitDatabaseManager.Instance.All.Select(m =>
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

        public IEnumerable<ENTITY> FindByIds<ENTITY, KEY>(KEY[] ids, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public void Remove<KEY>(KEY id)
        {
            throw new NotImplementedException();
        }

        public void Remove<KEY>(KEY id, DateTime datetime)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public DataTimeDataList<ENTITY> Select<ENTITY>(Query query)
        {
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
    }
}
