﻿using Shared.DataClasses;
using Shared.Utils;

namespace Server {
    public class ServerSendData {
        public static ServerSendData instance = new ServerSendData();

        public void SendWelcomeMessage(int index, UserData[] currentlyConnected) {
            BufferWriter bw = new BufferWriter();
            int instruction = 100;
            bw.WriteInt(instruction);
            bw.WriteInt(index);
            bw.WriteInt(currentlyConnected.Length);
            foreach (UserData userData in currentlyConnected) {
                bw.AddData(userData.ToBytes());
            }

            GameServer.instance.AddMessage(index, bw.ToArray());
        }
    }
}