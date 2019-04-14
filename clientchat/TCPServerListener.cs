using System;
using System.Net.Sockets;
using System.Threading;

namespace clientchat
{
    public class TCPServerListener
    {
        public TCPServerListener(){}

        private TcpListener listener = new TcpListener(sol.TcpIpAdress, sol.TcpPort);

        public void Listen(){
            listener.Start();

            while (true)
            {
                Socket client = listener.AcceptSocket();

                string tcpIpAndPort = string.Empty;
                byte[] data = new byte[100];
                int size = client.Receive(data);
                for (int i = 0; i < size; i++)
                    tcpIpAndPort += Convert.ToChar(data[i]);

                string[] separators = new string[] { ":" };
                var ipAndPort = tcpIpAndPort.Split(separators, StringSplitOptions.None);

                if(sol.TCPServerIpAdress == string.Empty)
                   sol.TCPServerIpAdress = ipAndPort[0];
                if(sol.TCPServerPort == 0)
                    sol.TCPServerPort = int.Parse(ipAndPort[1]);

            }
        }

        public void Close(){
            listener.Stop();
        }
    }
}
