using System;
using Game;
using Shared.DataClasses;
using Shared.Utils;

namespace Networking.MessageHandlers {
    public static class LoadSelfHandler {
        public static event EventHandler<PlayerData> Happened;
        public static void Handle(BufferReader reader) {
            PlayerData player = PlayerData.FromBytesFactory(reader);
            int index = UserManager.instance.selfUser.index;
            
            GameManager.instance.SpawnSelf(index, player);
            
            Happened?.Invoke(null, player);
        }
    }
}