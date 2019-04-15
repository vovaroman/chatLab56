using System;
using System.Net;

namespace serverchat.clientdiscovery
{
    public class Client
    {
        public Client(IPEndPoint ip, string guid, DateTime lastPing)
        {
            IP = ip;
            GUID = guid;
            LastPing = lastPing;
        }

        public IPEndPoint IP { get; set; }
        public string GUID { get; }

        public DateTime LastPing { get; set; }
    }
}