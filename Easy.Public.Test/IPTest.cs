using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Easy.Public.Test
{
    public class IPTest
    {
        [Test]
        public void GetIPTest()
        {
            string hostName = Dns.GetHostName();//本机名   
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            foreach (IPAddress ip in addressList)
            {
                //ip.AddressFamily


            }

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == 80)
                {
                    Console.WriteLine("已使用");
                    break;
                }
            }

        }
        [Test]
        public void IntranetIp4Test()
        {
            var value = IpHelper.IntranetIp4();
            Assert.AreEqual("172.18.11.65", value);
        }
        [Test]
        public void GetAvailablePortTest()
        {
            var value = IpHelper.IntranetIp4();

            IpHelper.GetAvailablePort(value, 80, 9999);

        }
    }
}
