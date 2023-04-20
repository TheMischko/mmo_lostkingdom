using Server.Model.ChatModel;
using Shared.DataClasses;
using Shared.Utils;

namespace Server.MessageSenders {
    public class NewChatMessagesSender {
        private const int instruction = 103;

        public static async void SendMessage(int index) {
            BufferWriter bw = new BufferWriter();
            bw.WriteInt(instruction);
            ChatHistoryFragment historyFragment = ChatModel.instance.GetNewMessages(index);
            if (historyFragment.messages.Count == 0) {
                return;
            }
            bw.WriteInt(historyFragment.messages.Count);
            foreach (ChatMessage message in historyFragment.messages) {
                bw.AddData(message.ToBytes());
            }
            await Network.instance.SendToClientAsync(index, bw.ToArray());
        }
    }
}