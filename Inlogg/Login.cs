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
    public class Login
    {
        private const string FilePath = "user.json";

        public void SignIn()
        {
            List<User> users = LoadUsers();

            if (users.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No saved users found. Please register first.[/]");
                return;
            }

            string name = AnsiConsole.Ask<string>("[green]Username:[/]");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Password:[/]")
                    .PromptStyle("red")
                    .Secret());

            // Check for matching user
            User loggedInUser = users.FirstOrDefault(u => u.Username == name && u.Password == password);

            if (loggedInUser == null)
            {
                AnsiConsole.MarkupLine("[red]Incorrect username or password![/]");
                return;
            }

            // 2FA code
            loggedInUser.Pending2FACode = Generate2FACode();
            AnsiConsole.MarkupLine($"[yellow](2FA code sent to {loggedInUser.Contact})[/]");

            AnsiConsole.MarkupLine($"[yellow]Your 2FA code is: [bold]{loggedInUser.Pending2FACode}[/][/]");

            string entered = AnsiConsole.Ask<string>("[green]Enter 2FA code:[/]");

            if (entered == loggedInUser.Pending2FACode)
            {
                AnsiConsole.MarkupLine($"[green]Welcome, {loggedInUser.Username}![/]");
                loggedInUser.Pending2FACode = "";

                UserMenu userMenu = new UserMenu(loggedInUser);
                userMenu.UserMe(); // Start the user menu
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Wrong 2FA code![/]");
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
