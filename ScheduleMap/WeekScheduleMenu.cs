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
                        "Create Week",            // 1
                        "Add Exercise to Week",   // 2
                        "Remove Exercise from Week", // 3
                        "View Week",              // 4
                        "View All Weeks",         // 5
                        "Delete Week",            // 6
                        "Back"                    // 0
                        })
                );

                // Handle selection
                switch (choice)
                {
                    case "Create Week":
                        scheduleService.AddWeek(); // Call method to create a week
                        break;

                    case "Add Exercise to Week":
                        scheduleService.AddExerciseToWeek(); // Call method to add exercise
                        break;

                    case "Remove Exercise from Week":
                        scheduleService.RemoveExerciseFromWeek(); // Call method to remove exercise
                        break;

                    case "View Week":
                        scheduleService.ViewWeek(); // Call method to view a single week
                        break;

                    case "View All Weeks":
                        scheduleService.ViewAllWeeks(); // Call method to view all weeks
                        break;

                    case "Delete Week":
                        scheduleService.DeleteWeek(); // Call method to delete a week
                        break;

                    case "Back":
                        running = false; // Exit menu
                        break;
                }
            }
        }
    }
}
