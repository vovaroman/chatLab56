using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace clientchat
{
    public class Invoker
    {
        //static Random r = 

        private int UDPPort = GetRandomUnusedPort();

        private static int TCPPort => 888;
        private static string localServerIp = GetLocalIPAddress();

        private static readonly Guid guid = Guid.NewGuid();
        private static readonly string appguid = guid.ToString().Remove(5);
        private int UDPServerPort => 20000;
        private static TCPServerListener tcpListener = new TCPServerListener();
        public Thread tcpListenThread = new Thread(() => tcpListener.Listen());
        public static UdpClient udpServer;
        private void PingServer()
        {
            udpServer = new UdpClient(UDPPort);
            string message = appguid + "||" + localServerIp + "||" + UDPPort;
            ping:
            udpServer.Send(Encoding.ASCII.GetBytes(message), message.Length, localServerIp, UDPServerPort);
            udpServer.BeginReceive(DataReceived, udpServer);


            if (sol.TCPServerIpAdress != string.Empty && sol.TCPServerPort != 0)
            {
                tcpListenThread.Abort();
                tcpListener.Close();
            }
            Thread.Sleep(2000);
            goto ping;
        }

        private void DataReceived(IAsyncResult ar)
        {
            var c = (UdpClient)ar.AsyncState;
            var receivedIpEndPoint = new IPEndPoint(IPAddress.Parse(GetLocalIPAddress()), UDPPort);
            var receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

            // Convert data to ASCII and print in console
            var receivedText = Encoding.ASCII.GetString(receivedBytes);
            //Console.Write(receivedIpEndPoint + ": " + receivedText + Environment.NewLine);
            Console.WriteLine(receivedText);
        }

        //private TcpListener tcpListener = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), 0);
        //private TcpClient client = null;

        private void SendTCPMessage(string message, string contactID)
        {
            try
            {
                TcpClient client = new TcpClient(sol.TCPServerIpAdress, sol.TCPServerPort);

                var port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                string data = contactID + "||" + message + "||" + appguid;
                NetworkStream nwStream = client.GetStream();
                byte[] bytesToSend = Encoding.ASCII.GetBytes(data);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            catch(Exception ex){
                Console.WriteLine("failed on message sending -" + ex.ToString());
            }


        }

        public void ReceiveMessages()
        {
            
        }


        public void StartChat()
        {
            var ping = new ThreadStart(PingServer);
            var doPing = new Thread(ping);

            tcpListenThread.Start();
            doPing.Start();


            //var tcpJob = new ThreadStart(tcpListener.Listen);
            //var doListenTcp = new Thread(tcpJob);
            //doListenTcp.Start();
            Console.WriteLine(
                $"'localServerIp' - {localServerIp} \n" +
                $"'UDPPort' - {UDPPort}\n" +
                $"'UDPServerPort' - {UDPServerPort}"
            );
            Console.WriteLine("ID: {0}", appguid);
            Console.WriteLine("Press any key to start chat with somebody... If u want group chat you first message should be - 'Start'");
            Console.ReadKey();
            string contactId = string.Empty;
            do
            {
                Console.WriteLine("Input contact id:");
                contactId = Console.ReadLine();
            } while (contactId == string.Empty);
            Console.WriteLine(@"Write messages below. Command to stop chat - 'Stop chat'");
            string message = string.Empty;
            do
            {
                message = appguid + " - " +  Console.ReadLine();
                SendTCPMessage(message,contactId);
            } while (message != "Stop chat");

        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Parse(localServerIp), 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }


    }
}
