using ShapeUp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShapeUp.Profille
{
    public class ProfilleMenu
    {
        private readonly User loggedInUser;          // The currently logged-in user
        private readonly ProfileManager profileManager; // Handles profile updates

        public ProfilleMenu(User user)
        {
            loggedInUser = user;                      // Store logged-in user
            profileManager = new ProfileManager();    // Initialize profile manager
        }

        public void ShowMenu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                // Spectre.Console menu for profile actions
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]Profile Menu[/]") // Menu title
                        .HighlightStyle(new Style(foreground: Color.Green, decoration: Decoration.Bold)) // Highlighted selection style
                        .AddChoices(new[] {
                        "View Profile",      // 1
                        "Update Profile",    // 2
                        "Back to User Menu"  // 0
                        })
                );

                switch (choice)
                {
                    case "View Profile":
                        ShowProffile();    // Show current user profile
                        Pause();            // Pause so user can read
                        break;

                    case "Update Profile":
                        profileManager.UpdateProfile(loggedInUser); // Update user profile
                        break;

                    case "Back to User Menu":
                        running = false;   // Exit menu
                        break;
                }
            }
        }

        private void ShowProffile()
        {
            Console.Clear();

            // Display profile info with colors
            AnsiConsole.MarkupLine("[bold cyan]User Profile[/]\n");
            AnsiConsole.MarkupLine($"[yellow]Username:[/] {loggedInUser.Username}");
            AnsiConsole.MarkupLine($"[yellow]Height:[/] {loggedInUser.Height} cm");
            AnsiConsole.MarkupLine($"[yellow]Weight:[/] {loggedInUser.Weight} kg");
            AnsiConsole.MarkupLine($"[yellow]Age:[/] {loggedInUser.Age}");
        }

        private void Pause()
        {
            // Pause so user can read
            AnsiConsole.MarkupLine("\n[grey]Press any key to return to the menu...[/]");
            Console.ReadKey();
        }
    }
}
