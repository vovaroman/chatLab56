using System.Net;

namespace serverchat.chat
{
    public class ChatUser
    {
        public string Guid { get; set; }
        public EndPoint Ip { get; set; }
    }
}