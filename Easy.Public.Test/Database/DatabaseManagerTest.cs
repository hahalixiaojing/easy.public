using Easy.Public.Database;
using Easy.Public.Database.DateTimeSplit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public.Test.Database
{
    public class DatabaseManagerTest
    {
        [NUnit.Framework.TestFixtureSetUp]
        public void SetUp()
        {
            var database1 = new MemoryListDateTimeSplitDatabase(new DateTime(2016, 1, 31), new DateTime(2016, 1, 1), 1);
            var database2 = new MemoryListDateTimeSplitDatabase(new DateTime(2016, 2, 29), new DateTime(2016, 2, 1), 2);
            var database3 = new MemoryListDateTimeSplitDatabase(new DateTime(2016, 3, 31), new DateTime(2016, 3, 1), 3);

            var seletor = new DateTimeSplitDatabaseSelector();
            seletor.Register(database1);
            seletor.Register(database2);
            seletor.Register(database3);

            var executor = new DefaultDateTimeDatabaseExecute(seletor);

            DatabaseManager.RegisterDataTimeDatabaseExecute("default", executor);
        }
        [NUnit.Framework.Test]
        public void AddTest()
        {
            var exector = DatabaseManager.DataTimeDatabaseExecute("default");

            var item = new Tuple<int,string,DateTime>(1,"a",DateTime.Now.AddMonths(-2));
            exector.Add(item, DateTime.Now.AddMonths(-2), (database, data) =>
            {
                var d = database.Database as List<Tuple<int, string, DateTime>>;
                d.Add(data);
            });


            var item2 = new Tuple<int, string, DateTime>(2, "b", DateTime.Now.AddMonths(-1));
            exector.Add(item2, DateTime.Now.AddMonths(-1), (database, data) =>
            {
                var d = database.Database as List<Tuple<int, string, DateTime>>;
                d.Add(data);
            });

            var item3 = new Tuple<int, string, DateTime>(3, "c", DateTime.Now);
            exector.Add(item3, DateTime.Now, (database, data) =>
            {
                var d = database.Database as List<Tuple<int, string, DateTime>>;
                d.Add(data);
            });

            var result1 = exector.FindBy<Tuple<int, string, DateTime>, int>(1, (database, id) =>
            {
                var d = database.Database as List<Tuple<int, string, DateTime>>;
                return d.Where(m => m.Item1 == id).FirstOrDefault();
            });

            Assert.AreEqual(item.Item1, result1.Item1);
            Assert.AreEqual(item.Item2, result1.Item2);
            Assert.AreEqual(item.Item3, result1.Item3);

            var result2 = exector.FindBy<Tuple<int, string, DateTime>, int>(2, (database, id) =>
            {
                var d = database.Database as List<Tuple<int, string, DateTime>>;
                return d.Where(m => m.Item1 == id).FirstOrDefault();
            });

            Assert.AreEqual(item2.Item1, result2.Item1);
            Assert.AreEqual(item2.Item2, result2.Item2);
            Assert.AreEqual(item2.Item3, result2.Item3);


            var datalist = exector.FindByIds(new int[2] { 2, 3 }, (database, ids) => {
                var d = database.Database as List<Tuple<int, string, DateTime>>;
                return d.Where(m => ids.Contains(m.Item1));
            });

            Assert.AreEqual(2, datalist.Count());
        }
    }
}
