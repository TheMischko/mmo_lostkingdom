using System;
using Networking.UI;
using Shared.DataClasses;
using Shared.Utils;
using UnityEngine;

namespace Networking.MessageSenders {
    public static class NewChatMessagesHandler {
        public static event EventHandler<ChatMessage[]> Happened;
        public static void Handle(BufferReader reader) {
            int count = reader.ReadInt();
            ChatMessage[] messages = new ChatMessage[count];
            for (int i = 0; i < count; i++) {
                ChatMessage message = new ChatMessage();
                message = (ChatMessage)message.FromBytes(reader);
                ChatManager.instance.AddMessage(message);
                messages[i] = message;
            }
            Happened?.Invoke(messages);
        }
    }
}