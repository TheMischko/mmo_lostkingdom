using Server.Model.ChatModel;
using Shared.DataClasses;
using Shared.Enums;
using Shared.Utils;

namespace Server.MessageHandlers {
    public class ChatMessageHandler {
        public static void Handle(int index, BufferReader reader) {
            int type = reader.ReadInt();
            string content = reader.ReadString();
            ChatMessage message = new ChatMessage(index, (ChatMessageType)type, content);
            ChatModel.instance.AddMessage(message);
        }
    }
}