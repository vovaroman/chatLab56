using System.Collections.Generic;

namespace serverchat.clientdiscovery
{
    public class Chat
    {
        public List<string> _messages = new List<string>();
        public List<ChatUser> _users = new List<ChatUser>();
        public bool GroupChat = false;
    }
}