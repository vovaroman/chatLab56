using System.Collections.Generic;
using serverchat.clientdiscovery;

namespace serverchat.chat
{
    public class Chat
    {
        public List<string> Messages = new List<string>();
        public List<ChatUser> Users = new List<ChatUser>();
        public bool GroupChat = false;
    }
}