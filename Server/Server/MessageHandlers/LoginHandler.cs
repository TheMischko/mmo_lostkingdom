using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Database.Schemas;
using Server.MessageSenders;
using Server.Model.ChatModel;
using Server.Model.UserModel;
using Server.Utils;
using ServiceStack;
using Shared.DataClasses;
using Shared.Enums;
using Shared.Utils;

namespace Server.MessageHandlers {
    public static class LoginHandler {
        public static async void Handle(int index, BufferReader reader) {
            string login = reader.ReadString();
            string password = reader.ReadString();

            // Handle login
            ResultInfo<User_Account> result = await UserModel.instance.Login(login, password);
            if (result.status == Status.Error) {
                await ErrorSender.SendMessage(index, ErrorType.LoginError, result.message);
                return;
            }

            Network.clients[index].userData.accountId = result.content.id;
            Network.clients[index].userData.name = result.content.login;
            
            await LoginSuccessfulSender.SendMessage(index, result.content.login);

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