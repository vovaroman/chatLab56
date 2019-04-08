using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using serverchat.clientdiscovery;
namespace serverchat
{
    public class Invoker
    {
        public void Invoke()
        {
            var serverListener = new ServerListener();
            var listener = new ThreadStart(serverListener.Listen);
            var receiveClients = new Thread(new ThreadStart(listener));

            var displayClients = new DisplayClients();
            var display = new Thread(() => {displayClients.Display(serverListener);});




            receiveClients.Start();
            display.Start();
            Console.ReadKey();
        }
    }
}
