using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Inlogg
{
    public class StartMenu
    {
        private readonly Register register;
        private readonly Login login;

        public StartMenu()
        {
            register = new Register();
            login = new Login();
        }

        public void Menu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]MAIN MENU[/]")
                        .PageSize(10)
                        .AddChoices(new[]
                        {
                        "Register new user",
                        "Login",
                        "Exit"
                        })
                        .HighlightStyle(new Style(Color.Green))
                );

                switch (choice)
                {
                    case "Register new user":
                        register.CreateUser();
                        Pause();
                        break;

                    case "Login":
                        login.SignIn();
                        Pause();
                        break;

                    case "Exit":
                        running = false;
                        break;
                }
            }
        }

        private void Pause()
        {
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
            Console.ReadKey();
        }
    }


}
