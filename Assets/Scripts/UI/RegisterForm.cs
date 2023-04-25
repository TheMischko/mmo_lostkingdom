using System;
using System.Text.RegularExpressions;
using Networking.MessageHandlers;
using Networking.MessageSenders;
using Shared.DataClasses;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class RegisterForm : MonoBehaviour {
        [SerializeField] private InputField usernameInput;
        [SerializeField] private InputField emailInput;
        [SerializeField] private InputField passwordInput;
        [SerializeField] private InputField passwordRepeatInput;
        [SerializeField] private Alert alert;
        
        private const string UsernameRegex = @"^[a-zA-Z0-9]+$";
        private const string EmailRegex = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public void SendForm() {
            string username = usernameInput.text;
            string email = emailInput.text;
            string password = passwordInput.text;
            string passwordRepeat = passwordRepeatInput.text;

            string error = GetValidationError(username, email, password, passwordRepeat);
            if (!string.IsNullOrEmpty(error)) {
                alert.ShowMessage(error, AlertType.Error);
                return;
            }
            alert.Hide();
            SuccessCreatedAccountHandler.Happened += OnAccountCreatedResponse;
            NewAccountSender.SendMessage(username, email, password);
        }

        private void OnAccountCreatedResponse(object sender, EventArgs e) {
            alert.ShowMessage("Your account was created.", AlertType.Success);
            SuccessCreatedAccountHandler.Happened -= OnAccountCreatedResponse;
        }

        private string GetValidationError(string username, string email, string password, string password2) {
            // Username
            if (string.IsNullOrEmpty(username)) {
                return "Username needs to be set.";
            }
            if (username.Length < 6) {
                return "Username is too short.";
            }
            if (!Regex.IsMatch(username, UsernameRegex)) {
                return "Username can use just alphabet characters and numbers.";
            }
            
            //Email
            if (string.IsNullOrEmpty(email)) {
                return "Email needs to be set.";
            }
            if (!Regex.IsMatch(email, EmailRegex)) {
                return "Email is in bad form.";
            }
            
            if (string.IsNullOrEmpty(password)) {
                return "Password needs to be set.";
            }

            // Passwords
            if (password.Length < 6) {
                return "Your password needs to be longer.";
            }
            if (string.IsNullOrEmpty(password2)) {
                return "You need to type the password second time.";
            }
            if (!password.Equals(password2)) {
                return "Passwords does not match.";
            }

            return string.Empty;
        }
    }
}