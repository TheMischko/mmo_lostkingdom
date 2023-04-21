using System;
using System.Collections.Generic;
using Networking.UI;
using Shared.DataClasses;
using Shared.Enums;
using UnityEngine;

namespace Networking {
    public class UserManager : MonoBehaviour {
        public static UserManager instance;
        
        public Dictionary<int, UserData> users;
        public UserData selfUser;

        [SerializeField] private bool gotSelf = false;
        [SerializeField] private int countPlayers = 0;

        public UserData GetUser(int index) {
            UserData user = users[index];
            if (user == null) {
                throw new Exception($"User with index {index} currently does not exist.");
            }
            Debug.Log("Getting user "+user.name);
            return user;
        }

        public void AddUser(UserData user) {
            if (users.ContainsKey(user.index)) {
                throw new Exception($"Cannot add user. User with index {user.index} already exists.");
            }
            users.Add(user.index, user);
            countPlayers += 1;
            ChatMessage message = new ChatMessage(
                    -1, 
                    ChatMessageType.Server, 
                    $"{user.name} just connected a game.",
                    DateTime.Now);
            ChatManager.instance.AddMessage(message);
        }

        public void RemoveUser(int index) {
            users.Remove(index);
        }

        public UserData[] GetUsers() {
            List<UserData> _users = new List<UserData>();
            foreach (KeyValuePair<int,UserData> user in users) {
                _users.Add(user.Value);
            }

            return _users.ToArray();
        }

        public int GetUserCount() {
            return users.Count;
        }

        public void SetSelf(UserData self) {
            selfUser = self;
            AddUser(self);
            gotSelf = true;
        }

        public UserData GetSelf() {
            return selfUser;
        }

        private void Awake() {
            instance = this;
            users = new Dictionary<int, UserData>();
        }
    }
}