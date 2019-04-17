using System;
using System.Net;

namespace serverchat.clientdiscovery
{
    public class Client
    {
        public Client(IPEndPoint ip, string guid, DateTime lastPing)
        {
            Ip = ip;
            Guid = guid;
            LastPing = lastPing;
        }

        public IPEndPoint Ip { get; set; }
        public string Guid { get; }

        public DateTime LastPing { get; set; }
    }
}