using System;
using System.Collections;
using System.Collections.Generic;
using Chat;
using Networking;
using Networking.MessageHandlers;
using Shared.DataClasses;
using Shared.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI {
    public class ChatManager : MonoBehaviour {
        public static ChatManager instance;
        
        [SerializeField] private ScrollRect chatOutputScroll;
        [SerializeField] private InputField inputText;

        [SerializeField] private GameObject chatPanel, textPrefab;
        
        private const int MaxMessages = 100;
        private const int MessagedToDelete = 10;

        private List<Message> messages;
        private List<Message> newMessages;
        
        private ChatMessageType messageType = ChatMessageType.Normal;
        private ChatMessageFormatter chatMessageFormatter;

        private bool isScrolling = false;

        private void Awake() {
            instance = this;
            chatMessageFormatter = new ChatMessageFormatter();
            inputText.onEndEdit.AddListener(OnMessageInputChange);
            //chatOutputScroll.onValueChanged.AddListener(OnScroll);
            messages = new List<Message>();
            newMessages = new List<Message>();

        }

        private void OnScroll(Vector2 position) {
            if (Mathf.Ceil(position.y*10)/10 == 1f) {
                isScrolling = false;
            }
            else {
                isScrolling = true;
            }
            
            Debug.Log(position);
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
            /*if (!isScrolling && newMessages.Count > 0) {
                foreach (string newMessage in newMessages) {
                    Debug.Log($"New messages: {newMessage}");
                    chatOutput.text += $"{newMessage}\n";
                }
                newMessages.Clear();
                Canvas.ForceUpdateCanvases();
            }

            if (!isScrolling) {
                chatOutputScroll.verticalNormalizedPosition = 1f;
            }

            if (!isScrolling && messages.Count > MaxMessages) {
                for (int i = 0; i < MessagedToDelete; i++) {
                    messages.Remove(messages[0]);
                }
            }*/
            if (newMessages.Count > 0) {
                foreach (Message newMessage in newMessages) {
                    GameObject newText = Instantiate(textPrefab, chatPanel.transform);
                    newMessage.textObject = newText.GetComponent<Text>();
                    newMessage.textObject.text = newMessage.text;
                    messages.Add(newMessage);
                }
                newMessages.Clear();
            }
        }

        /***
         * Functions
         */
        public void AddMessage(ChatMessage message) {
            if (messages.Count >= MaxMessages) {
                for (int i = 0; i < MessagedToDelete; i++) {
                    Destroy(messages[0].textObject.gameObject);
                    messages.Remove(messages[0]);
                }
            }
            string messageString = chatMessageFormatter.Format(message);

            Message messageObject = new Message {
                message = message,
                text = messageString
            };

            newMessages.Add(messageObject);
        }


        public void ClearChat() {
            messages.Clear();
            newMessages.Clear();
            int children = chatPanel.transform.childCount;
            for (int i = 0; i < children; i++) {
                Destroy(chatPanel.transform.GetChild(0).gameObject);
            }
        }
        
        public void OnScrollEnd() {
            isScrolling = false;
        }

    }

    [System.Serializable]
    public class Message {
        public ChatMessage message;
        public string text;
        public Text textObject;
    }
}