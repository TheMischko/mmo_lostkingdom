using Shared.Utils;

namespace Networking.MessageSenders {
    public static class LoginSender {
        private const int instruction = 2;

        public static void SendMessage(string login, string password) {
            BufferWriter bufferWriter = new BufferWriter();
            bufferWriter.WriteInt(instruction);
            bufferWriter.WriteString(login);
            bufferWriter.WriteString(password);
            byte[] data = bufferWriter.ToArray();
            Network.instance.SendData(data);
        }
    }
}