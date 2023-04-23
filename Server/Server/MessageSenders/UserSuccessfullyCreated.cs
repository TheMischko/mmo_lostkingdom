using System;
using Server.Model.ChatModel;
using Shared.DataClasses;
using Shared.Utils;

namespace Server.MessageSenders {
    public static class UserSuccessfullyCreated {
        private const int instruction = 200;

        public static async void SendMessage(int index) {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            bw.WriteInt(0);
            await Network.instance.SendToClientAsync(index, bw.ToArray());
        }
    }
}