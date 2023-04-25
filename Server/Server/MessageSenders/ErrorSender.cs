using System.Threading.Tasks;
using Shared.Enums;
using Shared.Utils;

namespace Server.MessageSenders {
    public static class ErrorSender {
        private const int instruction = 400;

        public static async Task SendMessage(int index, ErrorType type, string message) {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            bw.WriteInt((int)type);
            bw.WriteString(message);
            GameServer.instance.AddMessage(index, bw.ToArray());
        }
    }
}