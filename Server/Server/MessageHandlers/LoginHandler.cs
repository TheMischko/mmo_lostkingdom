using System;
using System.Runtime.InteropServices;
using Server.MessageSenders;
using Shared.Utils;

namespace Server.MessageHandlers {
    public static class LoginHandler {
        public static void Handle(int index, BufferReader reader) {
            string login = reader.ReadString();
            string password = reader.ReadString();
            Console.WriteLine($"{login} is logging in with password {password}");
            
            // Handle login
            
            LoginSuccessfulSender.SendMessage(index);
            
        }
    }
}