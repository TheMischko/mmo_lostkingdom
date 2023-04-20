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
        [SerializeField] private Text text;
        [SerializeField] private InputField inputText;
        private const int MaxMessages = 10;
        private string[] messages = new string[MaxMessages];
        private ChatMessageType messageType = ChatMessageType.Normal;

        public string testMessage = "";
        public bool send = false;

        private void Awake() {
            instance = this;
            inputText.onEndEdit.AddListener(OnMessageInputChange);
            
        }

        private async void OnMessageInputChange(string text) {
            Debug.Log(text);
            if (string.IsNullOrEmpty(text)) {
                return;
            }

            ChatMessage message = new ChatMessage(UserManager.instance.selfUser.index, messageType, text);
            await ClientSendData.instance.SendNewChatMessage(message);
            inputText.text = "";
        }

        private void Start() {
            ClearChat();
        }
        

        private void Update() {
            if (send) {
                AddMessage(testMessage);
                send = false;
            }
        }

        private void FixedUpdate() {
            text.text = string.Join("\n", messages);
        }


        public void ClearChat() {
            for (int i = 0; i < MaxMessages; i++) {
                messages[i] = "";
            }
        }

        public void AddMessage(string message) {
            if(string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
            ShiftByOne();
            messages[MaxMessages - 1] = message;
        }

        private void ShiftByOne() {
            for (int i = 0; i < MaxMessages-1; i++) {
                messages[i] = messages[i + 1];
            }
        }
        
    }
}