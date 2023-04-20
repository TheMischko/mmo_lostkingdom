using Shared.Utils;

namespace Networking.MessageSenders {
    public static class NewAccountSender {
        private const int instruction = 1;

        public static void SendMessage(string login, string email, string password) {
            BufferWriter bufferWriter = new BufferWriter();
            bufferWriter.WriteInt(instruction);
            bufferWriter.WriteString(login);
            bufferWriter.WriteString(email);
            bufferWriter.WriteString(password);
            byte[] data = bufferWriter.ToArray();
            Network.instance.SendData(data);
        }
    }
}