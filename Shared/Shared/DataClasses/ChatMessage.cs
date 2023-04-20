using System;
using Shared.Enums;
using Shared.Utils;

namespace Shared.DataClasses {
    public class ChatMessage:IBufferable {
        public int socketIndex;
        public ChatMessageType messageType;
        public string content;
        public DateTime timeReceived;

        public ChatMessage(int socketIndex, ChatMessageType messageType, string content, DateTime? timeReceived = null) {
            this.socketIndex = socketIndex;
            this.messageType = messageType;
            this.content = content;
            if(timeReceived != null)
                this.timeReceived = (DateTime)timeReceived;
        }

        public byte[] ToBytes() {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(socketIndex);
            bw.WriteInt((int)messageType);
            bw.WriteDateTime(timeReceived);
            bw.WriteString(content);
            
            return bw.ToArray();
        }

        public IBufferable FromBytes(BufferReader br) {
            int socketIndex = br.ReadInt();
            ChatMessageType messageType = (ChatMessageType)br.ReadInt();
            DateTime timeReceived = br.ReadDatetime();
            string content = br.ReadString();
            return new ChatMessage(socketIndex, messageType, content, timeReceived);
        }
    }
}