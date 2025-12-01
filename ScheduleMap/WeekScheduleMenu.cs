using ShapeUp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.ScheduleMap
{
    public class WeekScheduleMenu
    {
        private readonly ScheduleService scheduleService;

        // Constructor receives the logged-in user and initializes the ScheduleService
        public WeekScheduleMenu(User user)
        {
            scheduleService = new ScheduleService(user);
        }

        // Main menu loop
        public void Show()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                // Fancy Spectre Console title
                AnsiConsole.MarkupLine("[bold cyan]WEEK SCHEDULE MENU[/]");

                // Menu options with arrow keys
                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Choose an option:[/]")
                        .AddChoices(new[]
                        {
                        "Create Week",             // 1
                        "Add Exercise to Week",    // 2
                        "Add Meal to Week",        // 3
                        "Delete Exercise/Meal",    // 4
                        "View Week",               // 5
                        "View All Weeks",          // 6
                        "Delete Week",             // 7
                        "Back"                     // 0
                        })
                );

                // Handle selection
                switch (choice)
                {
                    case "Create Week":
                        scheduleService.AddWeek();
                        break;

                    case "Add Exercise to Week":
                        scheduleService.AddExerciseToWeek();
                        break;

                    case "Add Meal to Week":
                        scheduleService.AddMealToWeek();
                        break;

                    case "Delete Exercise/Meal":
                        scheduleService.RemoveItemFromWeek();
                        break;

                    case "View Week":
                        scheduleService.ViewWeek();
                        break;

                    case "View All Weeks":
                        scheduleService.ViewAllWeeks();
                        break;

                    case "Delete Week":
                        scheduleService.DeleteWeek();
                        break;

                    case "Back":
                        running = false;
                        break;
                }
            }
        }
    }
}
