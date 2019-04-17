using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace clientchat
{
    public class Sol
    {
        public static string TcpServerIpAdress = string.Empty;
        public static int TcpServerPort = 0;
        public static IPAddress TcpIpAdress => GetLocalIpAddress();
        public static int TcpPort => 30000;//GetRandomUnusedPort();

        public static IPAddress GetLocalIpAddress()
        {
            var ipv4Addresses = Array.FindAll(
                Dns.GetHostEntry(string.Empty).AddressList,
                a => a.AddressFamily == AddressFamily.InterNetwork);

            var localIp = ipv4Addresses.FirstOrDefault(x => x.ToString().Contains("192"));
            if (localIp != null)
                return localIp;
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}