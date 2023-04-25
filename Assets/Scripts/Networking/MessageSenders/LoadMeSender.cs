using System.ComponentModel;
using Shared.Utils;

namespace Networking.MessageSenders {
    public class LoadMeSender {
        private const int instruction = 21;

        public static void SendMessage() {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            Network.instance.SendData(bw.ToArray());
        }
    }
}