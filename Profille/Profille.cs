using ShapeUp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapeUp.Profille
{
    public class ProfileManager
    {
        private const string FilePath = "user.json";

        public void UpdateProfile(User LoggedInUser)
        {
            // Ask for height
            Console.Write("Height (cm): ");
            if (double.TryParse(Console.ReadLine(), out double height))
                LoggedInUser.Height = height;

            // Ask for weight
            Console.Write("Weight (kg): ");
            if (double.TryParse(Console.ReadLine(), out double weight))
                LoggedInUser.Weight = weight;

            // Ask for gender using arrow keys
            string gender = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select Gender:[/]")
                    .AddChoices(new[] { "Male", "Female", "Other" })
            );
            LoggedInUser.Gender = gender;

            // Ask for age
            Console.Write("Age: ");
            if (int.TryParse(Console.ReadLine(), out int age))
                LoggedInUser.Age = age;

            List<User> users = LoadUSers();

            // Find the correct user id
            var userUpdate = users.FirstOrDefault(u => u.ID == LoggedInUser.ID);
            if (userUpdate != null)
            {
                userUpdate.Height = LoggedInUser.Height;
                userUpdate.Weight = LoggedInUser.Weight;
                userUpdate.Gender = LoggedInUser.Gender;
                userUpdate.Age = LoggedInUser.Age;

                SaveUser(users);
                AnsiConsole.MarkupLine("[green]Profile updated successfully.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]User not found.[/]");
            }
        }

        private List<User> LoadUSers()
        {
            if (!File.Exists(FilePath))
                return new List<User>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private void SaveUser(List<User> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}

