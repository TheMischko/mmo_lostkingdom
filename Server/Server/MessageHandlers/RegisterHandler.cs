﻿using System;
using Server.MessageSenders;
using Server.Model.UserModel;
using Server.Utils;
using Shared.DataClasses;
using Shared.Utils;


namespace Server.MessageHandlers {
    public class RegisterHandler {
        public static async void Handle(int index, BufferReader reader) {
            string login = reader.ReadString();
            string email = reader.ReadString();
            string passwordRaw = reader.ReadString();
            Console.WriteLine("Creating a new account.");
            ResultInfo<long> result = await UserModel.instance.Register(login, email, passwordRaw);
            if (result.status == Status.Error) {
                // Handle error.
                Console.WriteLine("Error: " + result.message);
                return;
            }
            
            UserSuccessfullyCreated.SendMessage(index);
        }
    }
}