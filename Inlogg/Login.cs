using ShapeUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapeUp.Inlogg
{
    public class Login
    {
        private const string FilePath = "user.json";

        public void SignIn()
        {
            // Load all users from JSON
            List<User> users = LoadUsers();
            if (users.Count == 0)
            {
                Console.WriteLine("No saved users found. Please register first.");
                return;
            }

            Console.Write("Enter username: ");
            string name = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            // Find user matching username and password
            User user = users.FirstOrDefault(u => u.Username == name && u.Password == password);

            if (user == null)
            {
                Console.WriteLine("Incorrect username or password!");
                return;
            }

            // Generate 2FA code
            user.Pending2FACode = Generate2FACode();
            Console.WriteLine($"(2FA code sent to {user.Contact})");

            // For testing, we show the code directly
            Console.WriteLine($"Enter 2FA code: {user.Pending2FACode}");
            Console.Write("Enter 2FA code: ");
            string entered = Console.ReadLine();

            if (entered == user.Pending2FACode)
            {
                Console.WriteLine($"Welcome, {user.Username}!");
                user.Pending2FACode = "";
            }
            else
            {
                Console.WriteLine("Wrong 2FA code!");
            }
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
                // If JSON is invalid or empty
                return new List<User>();
            }
        }

        private string Generate2FACode()
        {
            var rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }
    }
}
