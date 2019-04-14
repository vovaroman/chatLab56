using System;
using System.Net;
using System.Net.Sockets;

namespace serverchat
{
    public static class Sol
    {

        public static IPAddress TcpIpAdress = GetLocalIPAddress();
        public static int TcpPort = GetRandomUnusedPort();

        private static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return IPAddress.Parse(ip.ToString());
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(GetLocalIPAddress(), 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
