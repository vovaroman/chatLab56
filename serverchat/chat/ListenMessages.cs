using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using serverchat.clientdiscovery;

namespace serverchat.chat
{
    public class ListenMessages
    {
        public void Listen(List<Chat> chats, List<Client> clients)
        {
            var tcpListener = new TcpListener(Sol.TcpIpAdress, Sol.TcpPort);
            tcpListener.Start();

            var socket = tcpListener.AcceptSocket();
            while (true)
            {
                var bytes = new byte[socket.ReceiveBufferSize];
                var connectionData = socket.Receive(bytes);
                var message = string.Empty;
                for (var i = 0; i < connectionData; i++)
                    message += Convert.ToChar(bytes[i]);
                var delimeters = new[] {"||"};
                var data = message.Split(delimeters, StringSplitOptions.None);
                var to = data[0];
                var body = data[1];
                var author = data[2];
                var tempChatUser = new ChatUser
                {
                    Guid = author,
                    Ip = socket.RemoteEndPoint
                };
                var isAuthorInClients =
                    clients.FirstOrDefault(x => x.Guid.StartsWith(author, StringComparison.CurrentCultureIgnoreCase));

                tempChatUser = CheckIfUserSholdBeProcessed(chats, clients, isAuthorInClients, tempChatUser, body, to);

                foreach (var chat in chats)
                foreach (var user in chat.Users)
                    Console.WriteLine("chat user - " + user.Ip);
                ChatListener.SendMessagesToUsers(tempChatUser, chats, clients);
                socket = tcpListener.AcceptSocket();
            }
        }

        private ChatUser CheckIfUserSholdBeProcessed(List<Chat> chats, List<Client> clients, Client isAuthorInClients, ChatUser tempChatUser,
            string body, string to)
        {
            if (isAuthorInClients == null) return tempChatUser;
            var isAuthorInChats = chats.FirstOrDefault(x => x.Users.Any(y => y.Guid == tempChatUser.Guid));
            if (isAuthorInChats != null)
            {
                tempChatUser = isAuthorInChats.Users.FirstOrDefault(x => x.Guid == tempChatUser.Guid);
                isAuthorInChats.Messages.Add(body);
            }
            else
            {
                var toInClients = clients.FirstOrDefault(x => x.Guid == to);
                var messageAuthor = clients.FirstOrDefault(x => x.Guid == tempChatUser.Guid);
                if (toInClients != null)
                {
                    var isGroupChat = false;
                    var groupChat = new Chat();
                    chats.ForEach(x =>
                    {
                        isGroupChat = x.Users.Any(y => y.Guid == to && x.GroupChat);
                        if (isGroupChat)
                            groupChat = x;
                    });
                    if (!isGroupChat)
                    {
                        if (messageAuthor == null) return tempChatUser;
                        var tempChat = new Chat
                        {
                            Messages = new List<string> {body},
                            Users = new List<ChatUser>
                            {
                                new ChatUser
                                {
                                    Guid = messageAuthor.Guid,
                                    Ip = messageAuthor.Ip
                                },
                                new ChatUser
                                {
                                    Guid = to,
                                    Ip = toInClients.Ip
                                }
                            }
                        };
                        chats.Add(tempChat);
                    }
                    else
                    {
                        if (messageAuthor != null)
                            groupChat.Users.Add(new ChatUser
                            {
                                Guid = messageAuthor.Guid,
                                Ip = messageAuthor.Ip
                            });
                    }
                }
                else
                {
                    toInClients = new Client(new IPEndPoint(IPAddress.Any, 0), to, DateTime.Now);
                    clients.Add(toInClients);
                    if (messageAuthor == null) return tempChatUser;
                    var tempChat = new Chat
                    {
                        Messages = new List<string> {body},
                        Users = new List<ChatUser>
                        {
                            new ChatUser
                            {
                                Guid = messageAuthor.Guid,
                                Ip = messageAuthor.Ip
                            },
                            new ChatUser
                            {
                                Guid = to,
                                Ip = tempChatUser.Ip
                            }
                        },
                        GroupChat = true
                    };
                    chats.Add(tempChat);
                }
            }
            return tempChatUser;
        }
    }
}