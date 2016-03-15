using System;

namespace Easy.Public.Database.DateTimeSplit
{
    public class Query
    {

        public Query()
        {
            this.OrderBy = OrderBy.DESC;
            this.Pageize = 20;
        }
        /// <summary>
        /// 最后一条记录所在的数据库索引
        /// </summary>
        public int DatabaseIndex { get; set; }
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime? Start { get; set; }
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime? End { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int Pageize { get; set; }
        /// <summary>
        /// 当前页码，第一页从1开始
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 数据编移位置（index索引位置）
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 订单排序类型
        /// </summary>
        public OrderBy OrderBy { get; set; }
    }

    public enum OrderBy
    {
        DESC,
        ASC
    }
}
