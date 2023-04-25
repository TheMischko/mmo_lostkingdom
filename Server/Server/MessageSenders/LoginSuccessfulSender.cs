using System.Threading.Tasks;
using Shared.DataClasses;
using Shared.Utils;

namespace Server.MessageSenders {
    public static class LoginSuccessfulSender {
        private const int instruction = 102;
        public static async Task SendMessage(int index, string username) {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            UserData userData = new UserData(index, username);
            bw.AddData(userData.ToBytes());
            
            await Network.instance.SendToClientAsync(index, bw.ToArray());
        }
    }
}