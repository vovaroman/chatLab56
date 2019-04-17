using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using serverchat.clientdiscovery;

namespace serverchat.chat
{
    public class ChatListener
    {
        public static List<Chat> Chats = new List<Chat>();

        public void StartChat(List<Client> clients)
        {
            var listenMessages = new ListenMessages();
            listenMessages.Listen(Chats, clients);
        }


        public static void SendMessagesToUsers(ChatUser author, List<Chat> chats, List<Client> clients)
        {
            //UdpClient udpServer = new UdpClient(ServerListener.UdpPort);
            //ServerListener.receiver
            foreach (var chat in chats)
            {
                chat.Users.ForEach(user =>
                {
                    var address = clients.FirstOrDefault(x => x.Guid == user.Guid);
                    if (address != null && author.Guid != user.Guid)
                        chat.Messages.ForEach(message =>
                        {
                            try
                            {
                                ServerListener.Receiver.Send(Encoding.ASCII.GetBytes(message), message.Length,
                                    address.Ip.Address.ToString(), address.Ip.Port);
                            }
                            catch (Exception ex)
                            {
                                // ignored
                            }

                            //udpServer.Send(Encoding.ASCII.GetBytes(message), message.Length, user.IP), UDPServerPort);)
                        });
                });
                chat.Messages = new List<string>();
            }
        }
    }
}