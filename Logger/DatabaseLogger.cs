using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace Easy.Public.MyLog
{
    public class DatabaseLogger : IMyLogger
    {
        private Type connectionType;
        private IFormatProvider formatProvider = new LogSqlFormatter();
        public String ConnectionString
        {
            get;
            set;
        }
        public String CommandSqlText
        {
            get;
            set;
        }

        public String ConnectionType
        {
            get;
            set;
        }

        #region IMyLogger Members
        public void Initialization()
        {
            if (!String.IsNullOrEmpty(this.ConnectionType))
            {
                this.connectionType = Type.GetType(this.ConnectionType);
            }
        }
        public void WriteLog(Log log)
        {
            if (String.IsNullOrEmpty(this.ConnectionString) || String.IsNullOrEmpty(this.CommandSqlText) || String.IsNullOrEmpty(this.ConnectionType))
            {
                return;
            }
            if (this.connectionType == null)
            {
                return;
            }
            
            using (DbConnection connection = Activator.CreateInstance(this.connectionType, this.ConnectionString) as DbConnection)
            {
                connection.Open();
                DbCommand commmand = connection.CreateCommand();
                commmand.Connection = connection;
                commmand.CommandText = String.Format(formatProvider, this.CommandSqlText, log);
                commmand.CommandType = CommandType.Text;
                try
                {
                    commmand.ExecuteNonQuery();
                }
                catch { }
                finally
                {
                    commmand.Dispose();
                }
            }
        }

        #endregion
    }
}
