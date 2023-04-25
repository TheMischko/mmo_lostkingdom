using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Database.Schemas;
using Database;
using Server.Utils;
using ServiceStack.OrmLite;

namespace Server.Model.UserModel {
    public class UserModel {
        public static UserModel instance = new UserModel();

        private readonly UserInputValidator inputValidator = new UserInputValidator();

        public async Task<ResultInfo<long>> Register(string username, string email, string passwordRaw) {
            // Validate
            string validationError = inputValidator.ValidateRegistration(username, email, passwordRaw);
            if (!string.IsNullOrEmpty(validationError)) {
                Console.WriteLine("Problem with registration - "+validationError);
                return new ResultInfo<long>(Status.Error, -1, validationError);
            }
            
            // Check database
            List<User_Account> users = await Database.Database.instance.connection.SelectAsync<User_Account>(
                u => u.login == username || u.email == email
            );
            if (users.Count > 0) {
                string databaseError = "User: User already exists.";
                Console.WriteLine("Problem with registration - " + databaseError);
                return new ResultInfo<long>(Status.Error, -1, databaseError);
            }
            
            // Add new user
            byte[] salt = GenerateSalt();
            string password = HashPassword(passwordRaw, salt);
            User_Account newAccount = new User_Account() {
                login = username,
                email = email,
                pasword = password,
                passwordSalt = Convert.ToBase64String(salt)
            };
            long id = await Database.Database.instance.connection.InsertAsync(newAccount);
            Console.WriteLine($"Registered a new user: {username}");
            
            return new ResultInfo<long>(Status.Ok, id);
        }

        public async Task<ResultInfo<User_Account>> Login(string username, string password) {
            string validationError = inputValidator.ValidateLogin(username, password);
            if (!string.IsNullOrEmpty(validationError)) {
                Console.WriteLine("Problem with login - "+validationError);
                return new ResultInfo<User_Account>(Status.Error, null, validationError);
            }

            List<User_Account> users = await Database.Database.instance.connection.SelectAsync<User_Account>(
                    u => u.login == username
            );

            if (users.Count == 0) {
                return new ResultInfo<User_Account>(Status.Error, null, "User: User not found.");
            }

            User_Account user = users.First();

            bool isPasswordMatching = DoesPasswordsMatch(password, user.pasword, user.passwordSalt);
            if (!isPasswordMatching) {
                return new ResultInfo<User_Account>(Status.Error, null, "Password: Wrong password.");
            }
            
            user.timeLoggedIn = DateTime.Now;
            await Database.Database.instance.connection.UpdateAsync<User_Account>(user);
            
            return new ResultInfo<User_Account>(Status.Ok, user);
        }

        private byte[] GenerateSalt() {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return salt;
        }

        private static string HashPassword(string passwordRaw, byte[] salt) {
            

            byte[] passwordSalted = Encoding.UTF8.GetBytes(passwordRaw).Concat(salt).ToArray();
            byte[] hashedBytes = new SHA256Managed().ComputeHash(passwordSalted);

            string hashedPassword = Convert.ToBase64String(hashedBytes);
            return hashedPassword;
        }

        private static bool DoesPasswordsMatch(string userPassword, string hashedPassword, string passwordSalt) {
            byte[] hashedStored = Convert.FromBase64String(hashedPassword);
            byte[] salt = Convert.FromBase64String(passwordSalt);
            
            byte[] passwordSalted = Encoding.UTF8.GetBytes(userPassword).Concat(salt).ToArray();
            byte[] hashedUserPassword = new SHA256Managed().ComputeHash(passwordSalted);

            return hashedUserPassword.SequenceEqual(hashedStored);
        }
    }
}