﻿using System;
using System.Collections.Generic;
using Shared.DataClasses;

namespace Server.Model.ChatModel {
    public struct ChatHistoryFragment {
        public int lastIndex;
        public List<ChatMessage> messages;
    }
    public class ChatModel {
        public static ChatModel instance = new ChatModel();
        private List<ChatMessage> chatHistory = new List<ChatMessage>();
        private Dictionary<int, int> usersLastIndices = new Dictionary<int, int>();

        public void AddMessage(ChatMessage message) {
            DateTime now = DateTime.Now;
            message.timeReceived = now;
            chatHistory.Add(message);
        }

        public void AddNewChatListener(int clientIndex) {
            usersLastIndices[clientIndex] = chatHistory.Count - 1;
        }

        public ChatHistoryFragment GetNewMessages(int clientIndex) {
            if (!usersLastIndices.ContainsKey(clientIndex)) {
                usersLastIndices[clientIndex] = -1;
            }
            
            int historyIndexFrom = usersLastIndices[clientIndex];
            int newLastIndex = chatHistory.Count - 1;
            // If no new messages, send empty List
            if (newLastIndex <= historyIndexFrom) {
                return new ChatHistoryFragment() {
                    lastIndex = newLastIndex, 
                    messages = new List<ChatMessage>()
                };
            }
            // Else return latest messages.
            List<ChatMessage> latestMessages = new List<ChatMessage>();
            for (int i = historyIndexFrom+1; i < newLastIndex+1; i++) {
                latestMessages.Add(chatHistory[i]);
            }

            usersLastIndices[clientIndex] = newLastIndex;

            return new ChatHistoryFragment() {
                lastIndex = newLastIndex,
                messages = latestMessages
            };
        }

    }
}