using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Networking.MessageHandlers;
using Networking.MessageSenders;
using Shared.DataClasses;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class LoginForm : MonoBehaviour {
        [SerializeField] private InputField login;
        [SerializeField] private InputField password;
        [SerializeField] private Alert alert;
        
        private const string UsernameRegex = @"^[a-zA-Z0-9]+$";

        public void SendForm() {
            string username = login.text;
            string passwordText = password.text;

            string error = GetValidationErrors(username, passwordText);
            if (!string.IsNullOrEmpty(error)) {
                alert.ShowMessage(error, AlertType.Error);
                return;
            }
            alert.Hide();
            LoginSender.SendMessage(username, passwordText);
            LoginSuccessfulHandler.Happened += OnSuccessfulLogin;
        }

        private void OnSuccessfulLogin(object sender, UserData e) {
            alert.ShowMessage("Login was successful.", AlertType.Success);
        }

        private string GetValidationErrors(string username, string passwordText) {
            if (string.IsNullOrEmpty(username)) {
                return "Tell us your username first, please.";
            }
            if (username.Length < 6) {
                return "That username is too short.";
            }
            if (!Regex.IsMatch(username, UsernameRegex)) {
                return "That username doesn't seem right.";
            }

            if (string.IsNullOrEmpty(passwordText)) {
                return "Your password is empty.";
            }
            if (passwordText.Length < 6) {
                return "Your password is too short.";
            }

            return string.Empty;
        }
    }
}

