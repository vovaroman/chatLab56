using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using serverchat.chat;

namespace serverchat.clientdiscovery
{
    public class ChatListener
    {

        public static List<Chat> chats = new List<Chat>();
        
        public void StartChat(List<Client> clients)
        {
            var listenMessages = new ListenMessages();
            listenMessages.Listen(chats,clients);
             
        }



        public static void SendMessagesToUsers(ChatUser author, List<Chat> chats, List<Client> clients){
            //UdpClient udpServer = new UdpClient(ServerListener.UdpPort);
            //ServerListener.receiver
            foreach(Chat chat in chats){
                chat._users.ForEach(user =>
                {
                    
                    string[] separators = new string[]{":"};
                    var ipAndPort = user.IP.ToString().Split(separators,StringSplitOptions.None);
                    var address = clients.FirstOrDefault(x => user.IP.ToString().Contains(x.IP.Port.ToString()));
                    if (address != null && author.Guid != user.Guid)
                    {
                        chat._messages.ForEach(message =>
                        {

                            try
                            {
                                ServerListener.receiver.Send(Encoding.ASCII.GetBytes(message), message.Length,
                                                             address.IP.Address.ToString(), address.IP.Port);
                            }
                            catch(Exception ex){
                            }
                            //udpServer.Send(Encoding.ASCII.GetBytes(message), message.Length, user.IP), UDPServerPort);)
                        });
                    }
                });
                chat._messages = new List<string>();
            }
        }
    }


    public class Chat
    {
        public List<ChatUser> _users = new List<ChatUser>();
        public List<string> _messages = new List<string>();
        public bool GroupChat = false;
    }
    public class ChatUser
    {
        public string Guid { get; set; }
        public System.Net.EndPoint IP { get; set; }
    }

}
