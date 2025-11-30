using ShapeUp.Models;
using Spectre.Console;
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
            // Use Spectre.Console for colored prompts
            string username = AnsiConsole.Ask<string>("[green]Enter username:[/]");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter password:[/]")
                    .PromptStyle("red")
                    .Secret());

            if (!ValidatePassword(password, out string reason))
            {
                AnsiConsole.MarkupLine($"[red]Weak password: {reason}[/]");
                return null;
            }

            string contact = AnsiConsole.Ask<string>("[green]Enter email or phone (for 2FA):[/]");
            string gender = AnsiConsole.Ask<string>("[green]Gender ! M/F[/]");
            double weight = AnsiConsole.Ask<double>("[green]Enter Weight ! (kg):[/]");
            double height = AnsiConsole.Ask<double>("[green]Enter Height ! (cm):[/]");
            double age = AnsiConsole.Ask<double>("[green]Enter Age ! :[/]");

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
                Contact = contact,
                Gender = gender,
                Weight = weight,
                Height = height,
                Age = age
            };

            // Add new user to list
            users.Add(user);

            // Save entire list back to JSON
            SaveUsers(users);

            AnsiConsole.MarkupLine($"[green]Welcome, {user.Username}! Registered successfully.[/]");
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
            AnsiConsole.MarkupLine("[yellow]User saved to JSON file.[/]");
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

