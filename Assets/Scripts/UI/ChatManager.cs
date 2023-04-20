using System;
using System.Collections;
using Networking.MessageHandlers;
using Shared.DataClasses;
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
        public UnityAction<string> OnTextInput;

        public string testMessage = "";
        public bool send = false;

        private void Awake() {
            instance = this;
            OnTextInput += AddSelfMessage;
            OnTextInput += s => StartCoroutine(ClearInput(s));
            inputText.onEndEdit.AddListener(OnTextInput);
            
        }

        private IEnumerator ClearInput(string message) {
            yield return new WaitForSeconds(2f);
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

        public void AddUserMessage(string username, string message) {
            AddMessage(String.Format("{0, 12}: {1}", username, message));
        }

        public void AddSelfMessage(string message) {
            UserData selfUser = UserManager.instance.selfUser;
            if(selfUser == null) return;
            string username = UserManager.instance.selfUser.name;
            AddUserMessage(username, message);
        }

        private void ShiftByOne() {
            for (int i = 0; i < MaxMessages-1; i++) {
                messages[i] = messages[i + 1];
            }
        }
        
    }
}