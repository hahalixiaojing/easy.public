using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Public
{
    public static class IpHelper
    {
        static readonly List<Tuple<uint, uint>> intranetRange = new List<Tuple<uint, uint>>();

        static IpHelper()
        {
            intranetRange.Add(new Tuple<uint, uint>(BitConverter.ToUInt32(IPAddress.Parse("10.0.0.0").GetAddressBytes(), 0),
               BitConverter.ToUInt32(IPAddress.Parse("10.255.255.255").GetAddressBytes(), 0)));

            intranetRange.Add(new Tuple<uint, uint>(BitConverter.ToUInt32(IPAddress.Parse("172.16.0.0").GetAddressBytes(), 0),
                BitConverter.ToUInt32(IPAddress.Parse("172.31.255.255").GetAddressBytes(), 0)));

            intranetRange.Add(new Tuple<uint, uint>(BitConverter.ToUInt32(IPAddress.Parse("192.168.0.0").GetAddressBytes(),0),
                BitConverter.ToUInt32(IPAddress.Parse("192.168.255.255").GetAddressBytes(),0)));
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
                uint tartgetIp = BitConverter.ToUInt32(IPAddress.Parse(ip).GetAddressBytes(), 0);
                var hasAny = intranetRange.Any(m => tartgetIp >= m.Item1 && tartgetIp <= m.Item2);

                if (hasAny)
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
            string[] ipList = Ip4List();
            foreach (var ip in ipList)
            {
                uint tartgetIp = BitConverter.ToUInt32(IPAddress.Parse(ip).GetAddressBytes(), 0);
                var hasAny = intranetRange.Any(m => tartgetIp >= m.Item1 && tartgetIp <= m.Item2);

                if (!hasAny)
                {
                    return ip;
                }
            }
            return string.Empty;
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

            var port = 80;// rand.Next(start, end + 1);
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
