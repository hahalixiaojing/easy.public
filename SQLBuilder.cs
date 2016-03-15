using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public
{
    /// <summary>
    /// SQL构建
    /// </summary>
    public class SQLBuilder
    {
        readonly IList<SqlSegment> sqlSegments = new List<SqlSegment>();

        readonly SqlSegment WHERE = new SqlSegment()
        {
            IsAppend = false,
            Sql = "WHERE"
        };

        public void AppendWhere()
        {
            sqlSegments.Add(WHERE);
        }

        public void Append(string sql)
        {
            sqlSegments.Add(new SqlSegment() { IsAppend = true, Sql = sql });
        }

        public void Append(bool condition, String prepend, String sql)
        {
            if (condition)
            {

                if (sqlSegments.Last().Sql != "WHERE")
                {
                    sqlSegments.Add(new SqlSegment()
                    {
                        IsAppend = true,
                        Sql = prepend
                    });
                }


                sqlSegments.Add(new SqlSegment() { IsAppend = true, Sql = sql });

                if (sqlSegments.Any(m => m.Sql == "WHERE"))
                {
                    SqlSegment segment = sqlSegments.Single(m => m.Sql == "WHERE");
                    segment.IsAppend = true;

                }
            }
        }

        public String Sql()
        {
            var sql = new StringBuilder();
            foreach (var keypair in sqlSegments)
            {
                if (keypair.IsAppend)
                {
                    sql.Append(" ");
                    sql.Append(keypair.Sql);
                }
            }
            return sql.ToString().Trim();
        }
    }

    class SqlSegment
    {
        public bool IsAppend
        {
            get;
            set;
        }
        public string Sql
        {
            get;
            set;
        }

    }
}
