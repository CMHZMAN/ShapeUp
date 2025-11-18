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
            List<User> users = LoadUsers();

            if (users.Count == 0)
            {
                Console.WriteLine("No saved users found. Please register first.");
                return;
            }

            Console.Write("Username: ");
            string name = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            // Check for matching user
            User loggedInUser = users.FirstOrDefault(u => u.Username == name && u.Password == password);

            if (loggedInUser == null)
            {
                Console.WriteLine("Incorrect username or password!");
                return;
            }

            // 2FA code
            loggedInUser.Pending2FACode = Generate2FACode();
            Console.WriteLine($"(2FA code sent to {loggedInUser.Contact})");

            Console.Write("Enter 2FA code: ");
            string entered = Console.ReadLine();

            if (entered == loggedInUser.Pending2FACode)
            {
                Console.WriteLine($"Welcome, {loggedInUser.Username}!");
                loggedInUser.Pending2FACode = "";

                UserMenu userMenu = new UserMenu(loggedInUser);
                userMenu.UserMe(); // Start the user menu
            }
            else
            {
                Console.WriteLine("Wrong 2FA code!");
            }
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(FilePath)) return new List<User>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private string Generate2FACode()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
