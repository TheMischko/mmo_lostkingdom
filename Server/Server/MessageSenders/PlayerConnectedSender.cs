using System.Threading.Tasks;
using Shared.DataClasses;
using Shared.Utils;

namespace Server.MessageSenders {
    public class PlayerConnectedSender {
        private const int instruction = 101;

        public static async Task SendMessage(int index, UserData newUser) {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            bw.AddData(newUser.ToBytes());

            await Network.instance.SendToClientAsync(index, bw.ToArray());
        }

        public static async Task BroadcastMessage(UserData newUser, int[] skipIndicies) {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            bw.AddData(newUser.ToBytes());
            await Network.instance.Broadcast(bw.ToArray(), skipIndicies);
        }
    }
}