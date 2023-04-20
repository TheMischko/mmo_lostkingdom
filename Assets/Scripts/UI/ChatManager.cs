using System;
using System.Collections;
using Networking.MessageHandlers;
using Shared.DataClasses;
using Shared.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Networking.UI {
    public class ChatManager : MonoBehaviour {
        public static ChatManager instance;
        
        [SerializeField] private Text chatOutput;
        [SerializeField] private InputField inputText;
        
        private const int MaxMessages = 100;
        private const int ShowMaxMessages = 10;
        
        private ChatMessage[] messages = new ChatMessage[MaxMessages];
        
        private ChatMessageType messageType = ChatMessageType.Normal;

        public string testMessage = "";
        public bool send = false;

        private void Awake() {
            instance = this;
            inputText.onEndEdit.AddListener(OnMessageInputChange);
            
        }

        private async void OnMessageInputChange(string text) {
            if (string.IsNullOrEmpty(text)) {
                return;
            }

            ChatMessage message = new ChatMessage(UserManager.instance.selfUser.index, messageType, text);
            await ClientSendData.instance.SendNewChatMessage(message);
            inputText.text = "";
        }

        /**
         * Mono behaviour loop
         */
        private void Start() {
            ClearChat();
        }

        private void FixedUpdate() {
            string[] showMessages = new string[ShowMaxMessages];
            int startingIndex = MaxMessages - ShowMaxMessages;
            for (int i = startingIndex; i < MaxMessages; i++) {
                string message = messages[i] == null ? "" : messages[i].content;
                int showMessageIndex = (i - startingIndex);
                showMessages[showMessageIndex] = message;
            }

            chatOutput.text = string.Join("\n", showMessages);
        }

        /***
         * Functions
         */
        public void AddMessage(ChatMessage message) {
            ShiftByOne();
            messages[MaxMessages-1] = message;
        }


        public void ClearChat() {
            for (int i = 0; i < MaxMessages; i++) {
                messages[i] = null;
            }

            chatOutput.text = string.Empty;
        }

        private void ShiftByOne() {
            for (int i = 0; i < MaxMessages-1; i++) {
                messages[i] = messages[i + 1];
            }
        }
        
    }
}