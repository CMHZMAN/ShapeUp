using ShapeUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapeUp.Inlogg
{
    public class Register
    {
        private const string FilePath = "user.json";

        public User CreateUser()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            if (!ValidatePassword(password, out string reason))
            {
                Console.WriteLine($"Weak password: {reason}");
                return null;
            }

            Console.Write("Enter email or phone (for 2FA): ");
            string contact = Console.ReadLine();

            // Load existing users
            List<User> users = LoadUsers();

            // Get next ID
            int newID = users.Count == 0 ? 1 : users.Max(u => u.ID) + 1;

            // Create new user
            var user = new User
            {
                ID = newID,
                Username = username,
                Password = password,
                Contact = contact
            };

            // Add new user to list
            users.Add(user);

            // Save entire list back to JSON
            SaveUsers(users);

            Console.WriteLine($"Welcome, {user.Username}! Registered successfully.");
            return user;
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(FilePath))
                return new List<User>();

            string json = File.ReadAllText(FilePath);
            try
            {
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                // In case the JSON is empty or invalid
                return new List<User>();
            }
        }

        private void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
            Console.WriteLine("User saved to JSON file.");
        }

        private bool ValidatePassword(string password, out string reason)
        {
            reason = "";

            if (password.Length < 4)
            {
                reason = "At least 4 characters required.";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                reason = "Must contain an uppercase letter.";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                reason = "Must contain a number.";
                return false;
            }

            if (!password.Any(ch => "!#¤%&".Contains(ch)))
            {
                reason = "Must contain a special character.";
                return false;
            }

            return true;
        }
    }
}

