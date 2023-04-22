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
            packets.Add(1, RegisterHandler.Handle);
            packets.Add(2, HandleLogin);
            packets.Add(3, ChatMessageHandler.Handle);
        }


        public void HandleMessage(int index, byte[] data) {
            BufferReader reader = new BufferReader(data);
            int instruction = reader.ReadInt();
            Console.WriteLine($"Got new message from {index} of type {instruction}");
            packets[instruction](index, reader);
        }

        private void HandleLogin(int index, BufferReader reader) {
            LoginHandler.Handle(index, reader);
        }
    }
}