using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Easy.Public.Test
{
    public class Test2
    {
        [Test]
        public void Test()
        {
            var demoData = new DemoData() { Name = "lixiaojing" };
            var demoData2 = new DemoData() { Name = "lixiaojing12123" };
            Call(demoData);
            Call(demoData2);

            Thread.Sleep(3000);
        }

        private void Call(DemoData a)
        {
            Task.Factory.StartNew(() =>
            {
                var invoker = new FaceOrderInvoker((p) =>
                {
                    Thread.Sleep(1000);
                    System.Diagnostics.Debug.WriteLine(a.Name);
                    return "";
                });

                invoker.Invoke();

            });

        }
    }

    public class DemoData
    {
        public string Name { get; set; }
    }


    public class FaceOrderInvoker
    {
        Func<DemoData, string> func;
        public FaceOrderInvoker(Func<DemoData, string> func)
        {
            this.func = func;
        }

        public void Invoke()
        {
            this.func.Invoke(null);
        }
    }
}
