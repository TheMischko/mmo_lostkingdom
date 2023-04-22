using System;
using System.Text.RegularExpressions;

namespace Server.Model.UserModel {
    public class UserInputValidator {
        private const string UsernameRegex = @"^[a-zA-Z0-9]+$";
        private const string EmailRegex = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        // Password regex test if password contains at least 8 characters, at least one uppercase letter,
        // at least one lowercase letter, at least one digit, and at least one special character.
        private const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        
        /// <summary>
        /// Validates inputs for registration and returns error message if there is any.
        /// </summary>
        /// <returns>Validation error message.</returns>
        public string ValidateRegistration(string username, string email, string password) {
            string usernameError = ValidatePartUsername(username);
            if (!string.IsNullOrEmpty(usernameError)) {
                return usernameError;
            }

            string emailError = ValidatePartEmail(email);
            if (!string.IsNullOrEmpty(emailError)) {
                return emailError;
            }

            string passwordError = ValidatePartPassword(password);
            if (!string.IsNullOrEmpty(passwordError)) {
                return passwordError;
            }
            
            return string.Empty;
        }

        public string ValidateLogin(string username, string password) {
            string usernameError = ValidatePartUsername(username);
            if (!string.IsNullOrEmpty(usernameError)) {
                return usernameError;
            }
            
            string passwordError = ValidatePartPassword(password);
            if (!string.IsNullOrEmpty(passwordError)) {
                return passwordError;
            }

            return string.Empty;
        }

        private static string ValidatePartUsername(string username) {
            if (username.Length < 4) {
                return "Username: Username is too short.";
            }
            if (username.Length > 32) {
                return "Username: Username is too long.";
            }
            if (!Regex.IsMatch(username, UsernameRegex)) {
                return "Username: Username is in bad form. It may contain forbidden characters.";
            }

            return string.Empty;
        }

        private static string ValidatePartPassword(string password) {
            if (password.Length == 0) {
                return "Password: Password was not set.";
            }

            return string.Empty;
        }

        private static string ValidatePartEmail(string email) {
            if (email.Length == 0) {
                return "Email: Email was not set.";
            }
            if (!Regex.IsMatch(email, EmailRegex)) {
                return "Email: Email is in bad form.";
            }

            return string.Empty;
        }
    }
}