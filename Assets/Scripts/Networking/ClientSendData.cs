using System;
using System.IO;
using System.Threading.Tasks;
using Networking.MessageSenders;
using Shared.DataClasses;
using Shared.Utils;

namespace Networking {
    public class ClientSendData {
        public static ClientSendData instance = new ClientSendData();
        public void SendAccount(string login, string email, string password) {
            NewAccountSender.SendMessage(login, email, password);
        }

        public void SendLogin(string login, string password) {
            LoginSender.SendMessage(login, password);
        }

        public async Task SendNewChatMessage(ChatMessage message) {
            await NewChatMessageSender.SendMessageAsync(message);
        }
    }
}
