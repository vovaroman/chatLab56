using System;
using System.Net.Sockets;

namespace clientchat
{
    public class TcpServerListener
    {
        private readonly TcpListener _listener = new TcpListener(Sol.TcpIpAdress, Sol.TcpPort);

        public void Listen()
        {
            _listener.Start();

            do
            {
                var client = _listener.AcceptSocket();
                var tcpIpAndPort = string.Empty;
                var data = new byte[100];
                var size = client.Receive(data);
                for (var i = 0; i < size; i++)
                    tcpIpAndPort += Convert.ToChar(data[i]);

                var separators = new[] {":"};
                var ipAndPort = tcpIpAndPort.Split(separators, StringSplitOptions.None);

                if (Sol.TcpServerIpAdress == string.Empty)
                    Sol.TcpServerIpAdress = ipAndPort[0];
                if (Sol.TcpServerPort == 0)
                    Sol.TcpServerPort = int.Parse(ipAndPort[1]);
            } while (Sol.TcpServerIpAdress == string.Empty);
        }

        public void Close()
        {
            _listener.Stop();
        }
    }
}