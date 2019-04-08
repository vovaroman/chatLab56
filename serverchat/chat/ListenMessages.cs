using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace serverchat.chat
{
    public class ListenMessages
    {
        
        public void Listen()
        {


            TcpListener tcpListener = new TcpListener(IPAddress.Any, 888);
            tcpListener.Start();

            Console.WriteLine("The server is running at port 8001...");
            Console.WriteLine("The local End point is  :" +
            tcpListener.LocalEndpoint );
            Console.WriteLine("Waiting for a connection.....");

            var s = tcpListener.AcceptSocket();
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            var b = new byte[100];
            var k = s.Receive(b);
            Console.WriteLine("Recieved...");
            for (var i = 0; i<k;i++)
            Console.Write(Convert.ToChar(b[i]));

            var asen = new ASCIIEncoding();
            s.Send(asen.GetBytes("The string was recieved by the server."));
            Console.WriteLine("\nSent Acknowledgement");

    /* clean up */
            s.Close();
            tcpListener.Stop();
        }

}
}
