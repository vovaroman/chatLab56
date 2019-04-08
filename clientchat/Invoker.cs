using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace clientchat
{
    public class Invoker
    {
        private int UDPPort => 20001;

        private static readonly Guid guid = Guid.NewGuid();
        private int UDPServerPort => 20000;
        private void PingServer()
        {
            ping:
            using (var myclient = new UdpClient(UDPPort))
                myclient.Send(Encoding.ASCII.GetBytes(guid.ToString()), 3, "localhost", UDPServerPort);
            Thread.Sleep(2000);
            goto ping;
        }


        public void StartChat()
        {
            var ping = new ThreadStart(PingServer);
            var doPing = new Thread(ping);
            doPing.Start();


        }
    }
}
