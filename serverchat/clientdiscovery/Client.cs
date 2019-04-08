using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace serverchat.clientdiscovery
{
    public class Client
    {
        public Client(IPEndPoint ip, string guid, DateTime lastPing)
        {
            this.IP = ip;
            this.GUID = guid;
            this.LastPing = lastPing;
        }

        public IPEndPoint IP { get; set; }
        public string GUID { get; }
        
        public DateTime LastPing { get; set; }
    }
}
