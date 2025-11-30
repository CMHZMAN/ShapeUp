using ShapeUp.Models;
using ShapeUp.Models.Exercises;
using ShapeUp.Models.Meal;
using ShapeUp.Models.Meal;
using ShapeUp.Profille;
using ShapeUp.Profille;
using ShapeUp.ScheduleMap;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp
{
    public class UserMenu
    {
        private readonly User loggedInUser;         // The currently logged-in user
        private readonly ProfileManager profileManager; // Handles profile updates
        private readonly MealMenu2 mealMenu;        // Handles meal menu
        private readonly ExerciseService exerciseService; // Handles exercises
        private readonly ScheduleService scheduleService; // Handles schedules
        private readonly MealMenu2 mealMenu2;      // Alternate meal menu (if needed)
        private readonly ProfilleMenu profilleMenu; // Profile sub-menu

        // Constructor receives the logged-in user
        public UserMenu(User user)
        {
            loggedInUser = user;                  // Store logged-in user
            profileManager = new ProfileManager(); // Initialize profile manager
            mealMenu = new MealMenu2(loggedInUser.ID); // Initialize meal menu
            exerciseService = new ExerciseService(loggedInUser); // Initialize exercise service
            scheduleService = new ScheduleService(loggedInUser); // Initialize schedule service
            mealMenu2 = new MealMenu2(loggedInUser.ID); // Initialize second meal menu
            profilleMenu = new ProfilleMenu(loggedInUser); // Initialize profile menu
        }

        // Main user menu loop
        public void UserMe()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                // Fancy Spectre.Console title
                AnsiConsole.MarkupLine("[bold cyan]USER MENU[/]");

                // Arrow-key menu using Spectre.Console
                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Choose an option:[/]")
                        .AddChoices(new[]
                        {
                        "Exercises",       // 1
                        "Meal Plan",       // 2
                        "View Schedule",   // 3
                        "Add Exercise to Schedule", // 4
                        "Profile",         // 5
                        "Log Out"          // 0
                        })
                );

                // Handle menu selection
                switch (choice)
                {
                    case "Exercises":
                        exerciseService.ViewExercises(); // View user exercises
                        break;

                    case "Meal Plan":
                        mealMenu2.ShowMenu(); // Open meal menu
                        break;

                    case "View Schedule":
                        scheduleService.ViewAllWeeks(); // View all scheduled weeks
                        break;

                    case "Add Exercise to Schedule":
                        scheduleService.AddExerciseToWeek(); // Add exercise to week
                        break;

                    case "Profile":
                        profilleMenu.ShowMenu(); // Open profile menu
                        break;

                    case "Log Out":
                        running = false;            // Exit menu
                        AnsiConsole.MarkupLine("[green]Logging out...[/]");
                        break;
                }
            }
        }
    }

}
