using Shared.DataClasses;
using Shared.Utils;

namespace Server.MessageSenders {
    public class SelfPlayerDataSender {
        private const int instruction = 121;

        public static void SendMessage(int index, PlayerData player) {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            bw.AddData(player.ToBytes());
            
            GameServer.instance.AddMessage(index, bw.ToArray());
        }
    }
}