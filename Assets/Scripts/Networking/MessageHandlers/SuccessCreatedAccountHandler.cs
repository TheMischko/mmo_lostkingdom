using System;
using Shared.DataClasses;
using Shared.Utils;
using UnityEngine;

namespace Networking.MessageHandlers {
    public static class SuccessCreatedAccountHandler {
        public static event EventHandler Happened;
        
        public static void Handle(BufferReader reader) {
            Happened?.Invoke(null, EventArgs.Empty);
        }
    }
}