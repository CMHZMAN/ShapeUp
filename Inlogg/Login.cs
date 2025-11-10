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
            public void SignIn()
            {
                // Load user from JSON file
                User user = LoadFromJson();
                if (user == null)
                {
                    Console.WriteLine("No saved user found. Please register first.");
                    return;
                }

                Console.Write("Enter username: ");
                string name = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                if (name != user.Username || password != user.Password)
                {
                    Console.WriteLine("Incorrect username or password!");
                    return;
                }

                
                user.Pending2FACode = Generate2FACode();
                Console.WriteLine($"(2FA code sent to {user.Contact})");
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

            private User LoadFromJson()
            {
                string filePath = "user.json";
                if (!File.Exists(filePath))
                {
                    return null;
                }

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<User>(json);
            }

            private string Generate2FACode()
            {
                var rnd = new Random();
                return rnd.Next(100000, 999999).ToString();
            }
        

    }
}
