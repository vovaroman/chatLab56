using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace serverchat.clientdiscovery
{
    public class ServerListener
    {
        public int UdpPort => 20000;

        public List<Client> Clients = new List<Client>();
        
        public void Listen()
        {
          
            UdpClient receiver = new UdpClient(UdpPort);
            receiver.BeginReceive(DataReceived, receiver);

        }
        private void DataReceived(IAsyncResult ar)
        {
            var c = (UdpClient)ar.AsyncState;
            var receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

            // Convert data to ASCII and print in console
            var receivedText = Encoding.ASCII.GetString(receivedBytes);
            //Console.Write(receivedIpEndPoint + ": " + receivedText + Environment.NewLine);
            var tempClient = new Client(receivedIpEndPoint, receivedText, DateTime.Now);
            var client = Clients.FirstOrDefault(x => x.GUID == receivedText);
            if(client == null)
                Clients.Add(new Client(receivedIpEndPoint, receivedText, DateTime.Now));
            else
                client.LastPing = DateTime.Now;

            // Restart listening for udp data packages
            c.BeginReceive(DataReceived, ar.AsyncState);
        }
    }
}
