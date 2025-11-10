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
        public User CreateUser()
        {
            // Ask user to enter a username
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            // Ask user to enter a password
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            // Check if password meets requirements
            if (!ValidatePassword(password, out string reason))
            {
                Console.WriteLine($"Weak password: {reason}");
                return null;
            }

            // Ask user to enter email or phone number for 2FA
            Console.Write("Enter email or phone (for 2FA): ");
            string contact = Console.ReadLine();

            // Create user object
            var user = new User
            {
                Username = username,
                Password = password,
                Contact = contact
            };

            // Save to JSON
            SaveToJson(user);

            Console.WriteLine($"Welcome, {user.Username}! Registered successfully.");
            return user;
        }

        // Save user to JSON file
        private void SaveToJson(User user)
        {
            string filePath = "user.json";
            string json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine("User saved to JSON file.");
        }

        // Check password rules
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

