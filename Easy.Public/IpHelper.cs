using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Easy.Public
{
    public static class IpHelper
    {
        //static readonly List<Tuple<IPAddress, IPAddress>> intranetRange = new List<Tuple<IPAddress, IPAddress>>();

        static IpHelper()
        {
            //intranetRange.Add(new Tuple<IPAddress, IPAddress>(IPAddress.Parse("10.0.0.0"),
            //   IPAddress.Parse("10.255.255.255")));

            //intranetRange.Add(new Tuple<IPAddress, IPAddress>(IPAddress.Parse("172.16.0.0"),
            //    IPAddress.Parse("172.31.255.255")));

            //intranetRange.Add(new Tuple<IPAddress, IPAddress>(IPAddress.Parse("192.168.0.0"),
            //    IPAddress.Parse("192.168.255.255")));

        }
        /// <summary>
        /// 获得电脑IP4地址列表
        /// </summary>
        /// <returns></returns>
        public static string[] Ip4List()
        {
            string hostName = Dns.GetHostName(); 
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);

            return addressList.Where(m => m.AddressFamily == AddressFamily.InterNetwork)
                .Select(m => m.ToString()).ToArray();
        }
        /// <summary>
        /// 获得一个内网IP
        /// </summary>
        /// <returns></returns>
        public static string IntranetIp4()
        {
            string[] ipList = Ip4List();
            foreach(var ip in ipList)
            {
                byte[] bytes = IPAddress.Parse(ip).GetAddressBytes();

                if (bytes[0] == 192 && bytes[1] == 168)
                {
                    return ip;
                }
                if(bytes[0] == 10)
                {
                    return ip;
                }

                if(bytes[0] == 172 && bytes[1]>=16 && bytes[1] <= 31)
                {
                    return ip;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 获得广域网IP
        /// </summary>
        /// <returns></returns>
        public static string InternetIp4()
        {
            string intranetIp4 = IntranetIp4();
            return Ip4List().Where(m => m != intranetIp4 && !m.StartsWith("169")).FirstOrDefault();

        }
        /// <summary>
        /// 127.0.0.1 IP地址
        /// </summary>
        /// <returns></returns>
        public static string LoopbackIp()
        {
            return IPAddress.Loopback.ToString();
        }

        /// <summary>
        /// 随机获得指定范围内可用的端口号
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="start">起始端口</param>
        /// <param name="end">结束端口</param>
        /// <returns></returns>
        public static int GetAvailablePort(string ip,int start, int end)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            var rand = new Random(Guid.NewGuid().GetHashCode());

            var port = rand.Next(start, end + 1);
            while (true)
            {
                bool istry = false;
                foreach (IPEndPoint endPoint in ipEndPoints)
                {
                    if (endPoint.Port == port && endPoint.Address.ToString() == ip)
                    {
                        istry = true;
                        break;
                    }
                }
                if (istry)
                {
                    port = rand.Next(start, end + 1);
                    istry = false;
                    continue;
                }
                return port;
            }
        }
    }
}
