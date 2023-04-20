using System;
using System.Collections.Generic;
using Server.MessageHandlers;
using Shared.Utils;

namespace Server {
    public class ServerHandleData {
        public static ServerHandleData instance = new ServerHandleData();

        private delegate void Packet_(int index, BufferReader reader);

        private Dictionary<int, Packet_> packets;

        public void InitMessages() {
            packets = new Dictionary<int, Packet_>();
            packets.Add(1, HandleNewRegistration);
            packets.Add(2, HandleLogin);
        }

        public void HandleMessage(int index, byte[] data) {
            BufferReader reader = new BufferReader(data);
            int instruction = reader.ReadInt();
            packets[instruction](index, reader);
        }

        private void HandleNewRegistration(int index, BufferReader reader) {
            string login = reader.ReadString();
            string email = reader.ReadString();
            string password = reader.ReadString();
            Console.WriteLine($"New account: {login}, {password}, {email}");
        }
        
        private void HandleLogin(int index, BufferReader reader) {
            LoginHandler.Handle(index, reader);
        }
    }
}