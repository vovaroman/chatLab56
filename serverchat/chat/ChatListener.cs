using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using serverchat.chat;

namespace serverchat.clientdiscovery
{
    public class ChatListener
    {
        public static List<Chat> chats = new List<Chat>();

        public void StartChat(List<Client> clients)
        {
            var listenMessages = new ListenMessages();
            listenMessages.Listen(chats, clients);
        }


        public static void SendMessagesToUsers(ChatUser author, List<Chat> chats, List<Client> clients)
        {
            //UdpClient udpServer = new UdpClient(ServerListener.UdpPort);
            //ServerListener.receiver
            foreach (var chat in chats)
            {
                chat._users.ForEach(user =>
                {
                    string[] separators = {":"};
                    var ipAndPort = user.IP.ToString().Split(separators, StringSplitOptions.None);
                    var address = clients.FirstOrDefault(x => x.GUID == user.Guid);
                    if (address != null && author.Guid != user.Guid)
                        chat._messages.ForEach(message =>
                        {
                            try
                            {
                                ServerListener.receiver.Send(Encoding.ASCII.GetBytes(message), message.Length,
                                    address.IP.Address.ToString(), address.IP.Port);
                            }
                            catch (Exception ex)
                            {
                            }

                            //udpServer.Send(Encoding.ASCII.GetBytes(message), message.Length, user.IP), UDPServerPort);)
                        });
                });
                chat._messages = new List<string>();
            }
        }
    }
}