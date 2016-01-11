using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Reflection;
using Easy.Public;
using System.Threading.Tasks;

namespace Easy.Public.MyLog
{
    public static class LogManager
    {
        private static Queue<Log> queue = new Queue<Log>(30);
        private static Semaphore semaphore = new Semaphore(30, 30);
        private static IList<IMyLogger> myLog;

        static LogManager()
        {
            myLog = new List<IMyLogger>();

            IsLogged = StringHelper.ToBoolean(ConfigurationManager.AppSettings[LogConfig.LOG_IS_LOGGED], false);
            if (IsLogged)
            {
                LogManager.myLog.Add(GetMyLogger());

                if (LogManager.myLog != null)
                {
                    foreach (var item in myLog)
                    {
                        item.Initialization();
                    }
                    LogManager.LogLevel = StringHelper.ToEnum<LogLevel>(ConfigurationManager.AppSettings[LogConfig.LOG_LEVEL], LogLevel.Verbose);
                    Thread thread = new Thread(new ThreadStart(LogManager.Work));
                    thread.Start();
                }
            }
        }

        public static void Register(IMyLogger log)
        {
            log.Initialization();
            myLog.Add(log);
        }

        private static IMyLogger GetMyLogger()
        {
            String provider = ConfigurationManager.AppSettings[LogConfig.LOG_LOGGER];

            if (String.IsNullOrEmpty(provider))
            {
                return new ConsoleLogger();
            }
            Type  type = Type.GetType(provider);
            if (type == null)
            {
                return new ConsoleLogger();
            }

            Object myLog = Activator.CreateInstance(type);

            if (myLog == null)
            {
                return new ConsoleLogger();
            }

            LogManager.SetLogObject(myLog, type);

            return myLog as IMyLogger;
        }

        private static void SetLogObject(Object @object,Type type)
        {
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo p in properties)
            {
                SetPropertyValue(@object, p);
            }
        }
        private static void SetPropertyValue(Object obj, PropertyInfo p)
        {
            String appSettingName = "log_p_" + p.Name.ToLower();

            String appSettingValue = ConfigurationManager.AppSettings[appSettingName];

            if (String.IsNullOrEmpty(appSettingValue))
            {
                return;
            }
            p.SetValue(obj, Convert.ChangeType(appSettingValue, p.PropertyType), null);
        }

        private static LogLevel LogLevel
        {
            get;
            set;
        }
        private static Boolean IsLogged
        {
            get;
            set;
        }
        public static void Verbose(String tag, object message)
        {
            if (IsLogged && LogLevel == LogLevel.Verbose)
            {
                LogManager.AddLog(new Log(tag, message, LogLevel.Verbose));
            }
        }
        public static void Info(String tag, object message)
        {
            if (IsLogged && LogManager.LogLevel <= LogLevel.Info)
            {
                LogManager.AddLog(new Log(tag, message, LogLevel.Info));
            }
        }
        public static void Warn(String tag, object message)
        {
            if (IsLogged && LogManager.LogLevel <= LogLevel.Warning)
            {
                LogManager.AddLog(new Log(tag, message, LogLevel.Warning));
            }
        }
        public static void Error(String tag, object message)
        {
            if (IsLogged && LogManager.LogLevel <= LogLevel.Error)
            {
                LogManager.AddLog(new Log(tag, message, LogLevel.Error));
            }
        }

        private static void AddLog(Log log)
        {
            if (semaphore.WaitOne(1000))
            {
                lock (queue)
                {
                    queue.Enqueue(log);
                    Monitor.Pulse(queue);
                }
            }
        }

        private static void Work()
        {
            while (true)
            {
                Log log = LogManager.Dequeue();
                if (myLog != null)
                {
                    try
                    {
                        foreach (var l in myLog)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                l.WriteLog(log);
                            });
                        }
                    }
                    catch (Exception e) { throw e; }
                }
            }
        }
        private static Log Dequeue()
        {
            lock (queue)
            {
                while (true)
                {
                    if (queue.Count > 0)
                    {
                        Log log = queue.Dequeue();
                        semaphore.Release();
                        return log;
                    }
                    Monitor.Wait(queue);
                }
            }
        }
    }
}
