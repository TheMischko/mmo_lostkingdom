using System.Threading.Tasks;
using Shared.DataClasses;
using Shared.Utils;

namespace Networking.MessageSenders {
    public static class NewChatMessageSender {
        private const int instruction = 3;

        public static async Task SendMessageAsync(ChatMessage message) {
            BufferWriter bufferWriter = new BufferWriter();
            bufferWriter.WriteInt(instruction);
            bufferWriter.WriteInt((int)message.messageType);
            bufferWriter.WriteString(message.content);
            byte[] data = bufferWriter.ToArray();
            Network.instance.SendData(data);
        }
    }
}