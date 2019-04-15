using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace clientchat
{
    public class Invoker
    {
        private static readonly string LocalServerIp = sol.GetLocalIpAddress().ToString();

        private static readonly Guid Guid = Guid.NewGuid();
        private static readonly string Appguid = Guid.ToString().Remove(5);
        private static readonly TcpServerListener TcpListener = new TcpServerListener();
        public static UdpClient UdpServer;

        private readonly int _udpPort = GetRandomUnusedPort();
        public static Thread TcpListenThread = new Thread(() => TcpListener.Listen());
        private static int UdpServerPort => 20000;

        private static string UdpServerAddress { get; set; }
        private DateTime LastPingJob;

        private void PingServer()
        {
            UdpServer = new UdpClient(_udpPort);
            var message = Appguid + "||" + LocalServerIp + "||" + _udpPort;
            while (true)
            {
                UdpServer.Send(Encoding.ASCII.GetBytes(message), message.Length, UdpServerAddress, UdpServerPort);
                UdpServer.BeginReceive(DataReceived, UdpServer);


                if (sol.TcpServerIpAdress != string.Empty && sol.TcpServerPort != 0)
                {
                    TcpListenThread.Abort();
                    TcpListener.Close();
                }
                LastPingJob = DateTime.Now;
                Thread.Sleep(2000);
            }
        }

        private void DataReceived(IAsyncResult ar)
        {
            var c = (UdpClient) ar.AsyncState;
            var receivedIpEndPoint = new IPEndPoint(sol.GetLocalIpAddress(), _udpPort);
            var receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

            // Convert data to ASCII and print in console
            var receivedText = Encoding.ASCII.GetString(receivedBytes);
            //Console.Write(receivedIpEndPoint + ": " + receivedText + Environment.NewLine);
            Console.WriteLine(receivedText);
        }

        //private TcpListener tcpListener = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), 0);
        //private TcpClient client = null;

        private static void SendTcpMessage(string message, string contactId)
        {
            try
            {
                var client = new TcpClient(sol.TcpServerIpAdress, sol.TcpServerPort);

                var port = ((IPEndPoint) client.Client.RemoteEndPoint).Port;
                var data = contactId + "||" + message + "||" + Appguid;
                var nwStream = client.GetStream();
                var bytesToSend = Encoding.ASCII.GetBytes(data);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed on message sending -" + ex);
            }
        }

        private void CheckPingJob()
        {
            if (!((DateTime.Now - LastPingJob).TotalSeconds < 5)) return;
            try
            {
                var listener = new TcpListener(sol.TcpIpAdress, sol.TcpPort);
                listener.Start();
                while (true)
                {
                    var client = listener.AcceptSocket();
                }
            }
            catch { }
        }

        public void StartChat()
        {
            Console.WriteLine("Input host:");
            UdpServerAddress = Console.ReadLine();
            var ping = new ThreadStart(PingServer);
            var doPing = new Thread(ping);
            doPing.Priority = ThreadPriority.Highest;
            var checkPingJob = new Thread(CheckPingJob);

            TcpListenThread.Start();
            doPing.Start();
            checkPingJob.Start();
            Console.WriteLine(
                $"'localServerIp' - {LocalServerIp} \n" +
                $"'UDPPort' - {_udpPort}\n" +
                $"'UDPServerPort' - {UdpServerPort}"
            );
            Console.WriteLine("ID: {0}", Appguid);
            Console.WriteLine(
                "Press any key to start chat with somebody... If u want group chat you first message should be - 'Start'");
            Console.ReadKey();
            string contactId;
            do
            {
                Console.WriteLine("Input contact id:");
                contactId = Console.ReadLine();
            } while (contactId == string.Empty);

            Console.WriteLine(@"Write messages below. Command to stop chat - 'Stop chat'");
            var message = string.Empty;
            do
            {
                message = Appguid + " - " + Console.ReadLine();
                SendTcpMessage(message, contactId);
            } while (message != "Stop chat");
        }

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Parse(LocalServerIp), 0);
            listener.Start();
            var port = ((IPEndPoint) listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}