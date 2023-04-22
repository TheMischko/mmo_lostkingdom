using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Server.MessageSenders;
using Server.Model.ChatModel;
using ServiceStack;
using Shared.DataClasses;
using Shared.Utils;

namespace Server.MessageHandlers {
    public static class LoginHandler {
        public static async void Handle(int index, BufferReader reader) {
            string login = reader.ReadString();
            string password = reader.ReadString();
            Console.WriteLine($"{login} is logging in with password {password}");
            
            // Handle login
            
            await LoginSuccessfulSender.SendMessage(index);

            ChatModel.instance.AddNewChatListener(index);
            
            List<UserData> users = Network.clients.Map(c => c.userData);
            foreach (UserData user in users) {
                if(user == null) continue;
                if(user.index == index) continue;
                await PlayerConnectedSender.SendMessage(index, user);
            }
            await PlayerConnectedSender.BroadcastMessage(Network.clients[index].userData, new []{index});
        }
    }
}