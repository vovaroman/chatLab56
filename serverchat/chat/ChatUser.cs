using System.Net;

namespace serverchat.clientdiscovery
{
    public class ChatUser
    {
        public string Guid { get; set; }
        public EndPoint IP { get; set; }
    }
}