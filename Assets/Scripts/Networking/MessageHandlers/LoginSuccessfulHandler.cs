using System;
using Shared.DataClasses;
using Shared.Utils;
using UnityEngine;

namespace Networking.MessageHandlers {
    public static class LoginSuccessfulHandler {
        public static event EventHandler<UserData> Happened;
        public static void Handle(BufferReader reader) {
            UserData user = UserData.FromBytesFactory(reader);
            UserManager.instance.SetSelf(user);
            Debug.Log("Login successful.");
            Happened?.Invoke(null, user);
        }
    }
}