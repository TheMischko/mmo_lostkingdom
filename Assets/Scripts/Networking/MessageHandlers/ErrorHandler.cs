using System;
using System.Collections.Generic;
using Shared.Enums;
using Shared.Utils;
using UnityEngine;

namespace Networking.MessageHandlers {
    public static class ErrorHandler {
        private static Dictionary<int, EventHandler<string>> typeListeners;
        
        static ErrorHandler() {
            typeListeners = new Dictionary<int, EventHandler<string>>();
            foreach (ErrorType type in Enum.GetValues(typeof(ErrorType))) {
                typeListeners[(int) type] = (sender, message) => {
                    Debug.Log($"ERROR: {message}");
                };
            }
        }
        public static void Handle(BufferReader reader) {
            int type = reader.ReadInt();
            string message = reader.ReadString();
            if (!typeListeners.ContainsKey(type)) {
                typeListeners[(int)ErrorType.Error]?.Invoke(null, message);
            }
            typeListeners[type]?.Invoke(null, message);
        }

        public static void AddListener(ErrorType type, EventHandler<string> handler) {
            typeListeners[(int) type] += handler;
        }

        public static void RemoveListener(ErrorType type, EventHandler<string> handler) {
            typeListeners[(int) type] -= handler;
        }
    }
}