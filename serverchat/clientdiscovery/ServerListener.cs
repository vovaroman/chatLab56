using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace serverchat.clientdiscovery
{
    public class ServerListener
    {
        public static UdpClient Receiver = new UdpClient(UdpPort);

        public List<Client> Clients = new List<Client>();
        public static int UdpPort => 20000;


        public void Listen()
        {
            Receiver.BeginReceive(DataReceived, Receiver);
        }

        private void DataReceived(IAsyncResult ar)
        {
            var c = (UdpClient) ar.AsyncState;
            var receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

            // Convert data to ASCII and print in console
            var receivedText = Encoding.ASCII.GetString(receivedBytes);
            string[] separators = {"||"};
            var message = receivedText.Split(separators, StringSplitOptions.None);
            var userUdp = new IPEndPoint(IPAddress.Parse(message[1]), int.Parse(message[2]));
            //Console.Write(receivedIpEndPoint + ": " + receivedText + Environment.NewLine);
            var client = Clients.FirstOrDefault(x => x.Guid == message[0]);
            if (client == null)
            {
                Clients.Add(new Client(userUdp, message[0], DateTime.Now));
                var tcpClient = new TcpClient(receivedIpEndPoint.Address.ToString(), 30000);
                var data = Sol.TcpIpAdress + ":" + Sol.TcpPort;
                var nwStream = tcpClient.GetStream();
                var bytesToSend = Encoding.ASCII.GetBytes(data);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            else
            {
                client.LastPing = DateTime.Now;
            }

            // Restart listening for udp data packages
            c.BeginReceive(DataReceived, ar.AsyncState);
        }
    }
}