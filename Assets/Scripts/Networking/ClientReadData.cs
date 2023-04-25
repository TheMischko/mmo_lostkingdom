
using System.Collections.Generic;
using Networking.MessageHandlers;
using Networking.MessageSenders;
using Shared.DataClasses;
using Shared.Utils;
using UnityEngine;

namespace Networking {
    public class ClientReadData {
        public static ClientReadData instance = new ClientReadData();

        private delegate void Packet_(BufferReader reader);

        private Dictionary<int, Packet_> packets;

        public void InitMessages() {
            packets = new Dictionary<int, Packet_>();
            packets.Add(100, HandleWelcomeMessage);
            packets.Add(102, LoginSuccessfulHandler.Handle);
            
            packets.Add(200, SuccessCreatedAccountHandler.Handle);

            packets.Add(400, ErrorHandler.Handle);
            LoginSuccessfulHandler.Happened += OnLoginAddHandlers;
        }

        public void HandleMessage(byte[] data) {
            
            BufferReader reader = new BufferReader(data);
            ulong serverTick = reader.ReadULong();
            int instruction = reader.ReadInt();
            Debug.Log($"Got new message of type {instruction}.");
            if (!packets.ContainsKey(instruction)) {
                return;
            }
            Packet_ handler = packets[instruction];
            handler(reader);
        }

        private void OnLoginAddHandlers(object sender, UserData e) {
            packets.Add(101, NewPlayerConnectedHandler.Handle);
            packets.Add(110, NewChatMessagesHandler.Handle);
        }

        private void HandleWelcomeMessage(BufferReader reader) {
            int index = reader.ReadInt();
            int numUsers = reader.ReadInt();
            List<UserData> users = new List<UserData>();
            for (int i = 0; i < numUsers; i++) {
                users.Add(UserData.FromBytesFactory(reader));
            }

            Debug.Log($"Connected, I received index: {index}.");
            Debug.Log($"There are totally {users.Count} players.");
        }
    }
}
