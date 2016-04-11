using System;
using System.Collections.Generic;

namespace Easy.Public.Database.DateTimeSplit
{
    public interface IDateTimeDatabaseExecute
    {
        /// <summary>
        /// 根据ID查询，在所有库执行操作
        /// </summary>
        /// <typeparam name="ENTITY"></typeparam>
        /// <typeparam name="KEY"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        ENTITY FindBy<ENTITY, KEY>(KEY id,Func<IDateTimeSplitDatabase,KEY,ENTITY> execute);
        /// <summary>
        /// 根据ID查询，指时间所在库的查询
        /// </summary>
        /// <typeparam name="ENTITY"></typeparam>
        /// <typeparam name="KEY"></typeparam>
        /// <param name="id"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        ENTITY FindBy<ENTITY, KEY>(KEY id, DateTime datetime, Func<IDateTimeSplitDatabase, KEY, ENTITY> execute);
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <typeparam name="ENTITY"></typeparam>
        /// <param name="entity"></param>
        /// <param name="datetime"></param>
        /// <param name="excute"></param>
        void Add<ENTITY>(ENTITY entity, DateTime datetime, Action<IDateTimeSplitDatabase, ENTITY> excute);

        /// <summary>
        /// 根据ID查询，在所有数据库查询
        /// </summary>
        /// <typeparam name="ENTITY"></typeparam>
        /// <typeparam name="KEY"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<ENTITY> FindByIds<ENTITY, KEY>(KEY[] ids, Func<IDateTimeSplitDatabase, KEY[], IEnumerable<ENTITY>> execute);
        /// <summary>
        /// 根据ID查询 指定时间范围内数据库中查询
        /// </summary>
        /// <typeparam name="ENTITY"></typeparam>
        /// <typeparam name="KEY"></typeparam>
        /// <param name="ids"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerable<ENTITY> FindByIds<ENTITY, KEY>(KEY[] ids, DateTime start, DateTime end, Func<IDateTimeSplitDatabase, KEY[], IEnumerable<ENTITY>> execute);
        /// <summary>
        /// 更新操作所有数据库都执行操作
        /// </summary>
        /// <typeparam name="ENTITY"></typeparam>
        /// <param name="entity"></param>
        void Update<ENTITY>(ENTITY entity, Action<IDateTimeSplitDatabase> execute);
        /// <summary>
        /// 更新操作，根据指定的时间定位数据库
        /// </summary>
        /// <typeparam name="ENTITY"></typeparam>
        /// <param name="entity"></param>
        /// <param name="datetime"></param>
        void Update<ENTITY>(ENTITY entity, DateTime datetime,Action<IDateTimeSplitDatabase, ENTITY> execute);
        /// <summary>
        /// 移除所有的数据库数据
        /// </summary>
        void RemoveAll(Action<IDateTimeSplitDatabase> execute);
        /// <summary>
        /// 移除指定ID的数所在，在所有数据都要执行
        /// </summary>
        /// <typeparam name="KEY"></typeparam>
        /// <param name="id"></param>
        void Remove<KEY>(KEY id, Action<IDateTimeSplitDatabase, KEY> execute);
        /// <summary>
        /// 删除操作，根据指定的时间定位数据库
        /// </summary>
        /// <typeparam name="KEY"></typeparam>
        /// <param name="id"></param>
        /// <param name="datetime"></param>
        void Remove<KEY>(KEY id, DateTime datetime, Action<IDateTimeSplitDatabase, KEY> execute);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="ENTITY"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        DataTimeDataList<ENTITY> Select<ENTITY>(Query query, Func<IDateTimeSplitDatabase, Query, long, IEnumerable<ENTITY>> dataExecute,
            Func<IDateTimeSplitDatabase, Query, long> countExecute);
        /// <summary>
        /// 聚合计算，count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Int64 Count(Query query, Func<IDateTimeSplitDatabase, Query, Int64> execute);
        /// <summary>
        /// 聚合计算 count
        /// </summary>
        /// <returns></returns>
        Int64 Count(Func<IDateTimeSplitDatabase, Int64> execute);
    }
}
