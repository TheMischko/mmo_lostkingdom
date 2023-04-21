using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Networking;
using Shared.DataClasses;
using Shared.Enums;
using UnityEngine;

namespace Chat {
    public class ChatMessageFormatter {
        private UserManager userManager;
        private delegate string MessageFormatterMethod(ChatMessage message);

        private Dictionary<ChatMessageType, MessageFormatterMethod> messageFormatters;

        private string messageDateFormat = "hh:mm";

        public ChatMessageFormatter() {
            messageFormatters =  new Dictionary<ChatMessageType, MessageFormatterMethod>();
            
            messageFormatters.Add(ChatMessageType.Normal, FormatNormalMessage);
            messageFormatters.Add(ChatMessageType.Server, FormatServerMessage);
            messageFormatters.Add(ChatMessageType.Admin, FormatAdminMessage);
        }

        public string Format([CanBeNull] ChatMessage message) {
            if (message == null) {
                return "";
            }
            return messageFormatters[message.messageType](message);
        }

        private static string ColorText(string text, Color color) {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";
        }

        private string FormatNormalMessage(ChatMessage message) {
            return string.Format("{0} {1}: {2}", 
                ColorText(message.timeReceived.ToString(messageDateFormat), ChatColors.dateColor),
                ColorText(UserManager.instance.GetUser(message.socketIndex).name, ChatColors.userColor),
                ColorText(message.content, ChatColors.messageColor)
                );
        }
        private string FormatAdminMessage(ChatMessage message) {
            return ColorText(
                string.Format("<b>{0} [GM]{1}: {2}</b>",
                        message.timeReceived.ToString(messageDateFormat),
                        UserManager.instance.GetUser(message.socketIndex).name,
                        message.content
                        ),
                ChatColors.adminColor
            );
        }

        private string FormatServerMessage(ChatMessage message) {
            return ColorText($"{message.timeReceived.ToString(messageDateFormat)} <i>Server: {message.content}</i>", 
                ChatColors.serverColor);
        }
    }
}