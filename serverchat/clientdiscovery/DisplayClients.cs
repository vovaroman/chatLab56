﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace serverchat.clientdiscovery
{
    public class DisplayClients
    {
        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public void Display(ServerListener serverListener)
        {
             
            todo:
            Console.Clear();
            Console.WriteLine("Starting Upd receiving on port: " + serverListener.UdpPort);
            Console.WriteLine("-------------------------------\n");
            Console.WriteLine("Current clients:\n");

            lock (serverListener.Clients)
            {
                foreach (var client in serverListener.Clients)
                {
                    if ((DateTime.Now - client.LastPing).TotalSeconds < 3)
                    {
                        Console.WriteLine($"IP - {client.IP} Client ID - {client.GUID}\n");
                    }
                }
            }
            

            Thread.Sleep(3000);
            goto todo;
        }
    }
}
