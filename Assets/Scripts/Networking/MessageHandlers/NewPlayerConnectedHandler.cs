using Shared.DataClasses;
using Shared.Utils;
using UnityEngine;

namespace Networking.MessageHandlers {
    public static class NewPlayerConnectedHandler {
        public static void Handle(BufferReader reader) {
            UserData user = UserData.FromBytesFactory(reader);
            UserManager.instance.AddUser(user);
            Debug.Log($"New user logged in: {user.name}");
        }
    }
}