using System;
using System.Net.Sockets;
using System.Threading;

namespace clientchat
{
    public class TcpServerListener
    {
        public TcpServerListener(){}

        private readonly TcpListener _listener = new TcpListener(sol.TcpIpAdress, sol.TcpPort);

        public void Listen(){
            _listener.Start();

            do
            {
                var client = _listener.AcceptSocket();

                var tcpIpAndPort = string.Empty;
                var data = new byte[100];
                var size = client.Receive(data);
                for (var i = 0; i < size; i++)
                    tcpIpAndPort += Convert.ToChar(data[i]);

                var separators = new string[] {":"};
                var ipAndPort = tcpIpAndPort.Split(separators, StringSplitOptions.None);

                if (sol.TcpServerIpAdress == string.Empty)
                    sol.TcpServerIpAdress = ipAndPort[0];
                if (sol.TcpServerPort == 0)
                    sol.TcpServerPort = int.Parse(ipAndPort[1]);

            } while (sol.TcpServerIpAdress == string.Empty);
        }

        public void Close(){
            _listener.Stop();
        }
    }
}
