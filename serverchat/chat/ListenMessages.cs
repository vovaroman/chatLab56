using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using serverchat.clientdiscovery;

namespace serverchat.chat
{
    public class ListenMessages
    {



        public void Listen(List<Chat> chats,List<Client> clients)
        {


            TcpListener tcpListener = new TcpListener(Sol.TcpIpAdress, Sol.TcpPort);
            tcpListener.Start();

            var socket = tcpListener.AcceptSocket();
            while(socket != null)
            {
                var bytes = new byte[socket.ReceiveBufferSize];
                var connectionData = socket.Receive(bytes);
                string message = string.Empty;
                for (var i = 0; i < connectionData; i++)
                    message += Convert.ToChar(bytes[i]);
                var delimeters = new string[] { "||" };
                var data = message.Split(delimeters,StringSplitOptions.None);
                string to = data[0];
                string body = data[1];
                string author = data[2];
                var tempChatUser = new ChatUser()
                {
                    Guid = author,
                    IP = socket.RemoteEndPoint
                };
                var isAuthorInClients =
                    clients.FirstOrDefault(x => x.GUID.StartsWith(author, StringComparison.CurrentCultureIgnoreCase));

                if(isAuthorInClients != null)
                {
                    var isAuthorInChats = chats.FirstOrDefault(x => x._users.Any(y => y.Guid == tempChatUser.Guid));
                    if(isAuthorInChats != null)
                    {
                        tempChatUser = isAuthorInChats._users.FirstOrDefault(x => x.Guid == tempChatUser.Guid);
                        isAuthorInChats._messages.Add(body);
                    }
                    else{
                        var toInClients = clients.FirstOrDefault(x => x.GUID == to);
                        var messageAuthor = clients.FirstOrDefault(x => x.GUID == tempChatUser.Guid);
                        if (toInClients != null)
                        {
                            var isGroupChat = false;
                            var groupChat = new Chat();
                            chats.ForEach(x =>
                            {
                                isGroupChat = x._users.Any(y => y.Guid == to &&  x.GroupChat);
                                if (isGroupChat)
                                    groupChat = x;
                            });
                            if (!isGroupChat)
                            {
                                var tempChat = new Chat()
                                {
                                    _messages = new List<string>() { body },
                                    _users = new List<ChatUser> {
                                    new ChatUser(){
                                        Guid = messageAuthor.GUID,
                                        IP = messageAuthor.IP
                                    },
                                    new ChatUser(){
                                        Guid = to,
                                        IP = toInClients.IP
                                    }
                                }

                                };
                                chats.Add(tempChat);
                            }
                            else{
                                groupChat._users.Add(new ChatUser()
                                {
                                    Guid = messageAuthor.GUID,
                                    IP = messageAuthor.IP
                                });
                            }
                        }
                        else{
                            toInClients = new Client(new IPEndPoint(IPAddress.Any, 0), to, DateTime.Now);
                            clients.Add(toInClients);
                            var tempChat = new Chat()
                            {
                                _messages = new List<string>() { body },
                                _users = new List<ChatUser> {
                                    new ChatUser(){
                                        Guid = messageAuthor.GUID,
                                        IP = messageAuthor.IP
                                    },
                                    new ChatUser(){
                                        Guid = to,
                                        IP = tempChatUser.IP
                                    }
                                },
                                GroupChat = true
                            };
                            chats.Add(tempChat);

                        }
                    }
                }
                foreach (var chat in chats)
                    foreach (var user in chat._users)
                        Console.WriteLine("chat user - " + user.IP);
                ChatListener.SendMessagesToUsers(tempChatUser, chats, clients);
                socket = tcpListener.AcceptSocket();

                
            }
           
           
        }

}
}
