using System;
using System.Threading;
using serverchat.chat;
using serverchat.clientdiscovery;

namespace serverchat
{
    public class Invoker
    {
        public void Invoke()
        {
            var serverListener = new ServerListener();
            var listener = new ThreadStart(serverListener.Listen);
            var receiveClients = new Thread(listener);

            var displayClients = new DisplayClients();
            var display = new Thread(() => { displayClients.Display(serverListener); });

            var chat = new ChatListener();
            var chatThread = new Thread(() => { chat.StartChat(serverListener.Clients); });

            receiveClients.Start();
            display.Start();
            chatThread.Start();
            Console.ReadKey();
        }
    }
}