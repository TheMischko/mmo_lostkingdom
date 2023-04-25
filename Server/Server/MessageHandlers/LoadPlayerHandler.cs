using Server.MessageSenders;
using Server.Model.UserModel;
using Shared.DataClasses;
using Shared.Utils;

namespace Server.MessageHandlers {
    public static class LoadPlayerHandler {
        public static async void Handle(int index, BufferReader br) {
            // Load player
            PlayerData player = GetPlayer(Network.clients[index].userData);
            SelfPlayerDataSender.SendMessage(index, player);
        }

        private static PlayerData GetPlayer(UserData user) {
            PlayerStats stats = new PlayerStats(10, 10, 3);
            PlayerData player = new PlayerData(user.name, 0f, 0f, "", stats);
            return player;
        }
    }
}