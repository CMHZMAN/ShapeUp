using ShapeUp.Models;
using ShapeUp.Models.Exercises;
using ShapeUp.Models.Meal;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.ScheduleMap
{
    public class ScheduleService
    {
        private User loggedInUser;                  // The currently logged-in user
        private readonly UserDataService userDataService; // Helper class for loading/saving user JSON
        private readonly MealManager mealManager;   // Manages meals for the user

        // Constructor: store the logged-in user, prepare JSON helper, and initialize MealManager
        public ScheduleService(User user)
        {
            loggedInUser = user;
            userDataService = new UserDataService();
            mealManager = new MealManager(user.ID); // Connect to user's meal JSON file
        }

        // ADD NEW WEEK
        public void AddWeek()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]Add New Week Schedule[/]");

            // Ask user for the week number (1-52)
            int weekNumber = AnsiConsole.Ask<int>("[yellow]Enter week number (1–52):[/]");

            // Determine a new unique week ID
            int newWeekId = loggedInUser.WeeklyPlans.Any()
                ? loggedInUser.WeeklyPlans.Max(w => w.Id) + 1
                : 1;

            // Create a new Schedule object
            var newWeek = new Schedule
            {
                Id = newWeekId,
                WeekNumber = weekNumber,
                Items = new List<ScheduledItem>() // Empty list initially
            };

            // Add the new week to the user's plans
            loggedInUser.WeeklyPlans.Add(newWeek);

            // Save changes to the user JSON
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine($"[green]Week {weekNumber} added with ID {newWeekId}![/]");
            Console.ReadKey();
        }

        // VIEW ALL WEEKS

        public void ViewAllWeeks()
        {
            Console.Clear();

            // Check if user has any scheduled weeks
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks scheduled yet.[/]");
                Console.ReadKey();
                return;
            }

            // Display all weeks with their ID, week number, and number of items
            AnsiConsole.MarkupLine("[bold green]All Scheduled Weeks:[/]");
            foreach (var week in loggedInUser.WeeklyPlans.OrderBy(w => w.WeekNumber))
            {
                AnsiConsole.MarkupLine($"[yellow]ID:[/] {week.Id}, Week Number: [cyan]{week.WeekNumber}[/], Items: [green]{week.Items.Count}[/]");
            }

            Console.ReadKey();
        }

        // DELETE A WEEK
        public void DeleteWeek()
        {
            Console.Clear();

            // Check if user has any weeks
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks to delete.[/]");
                Console.ReadKey();
                return;
            }

            // Add "Go Back" option at the bottom of the selection list
            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a week to delete:[/]")
                    .AddChoices(loggedInUser.WeeklyPlans
                        .OrderBy(w => w.WeekNumber)
                        .Select(w => $"ID {w.Id} | Week {w.WeekNumber}")
                        .Concat(new[] { "0 | Go Back" }) // <-- Go Back option
                        .ToList())
            );

            // If user chose Go Back, exit the method
            if (weekChoice.StartsWith("0"))
                return;

            // Parse the selected week ID and remove it from user's list
            int weekId = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());
            loggedInUser.WeeklyPlans.RemoveAll(w => w.Id == weekId);

            // Save changes to JSON
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine("[green]Week deleted![/]");
            Console.ReadKey();
        }

        // ADD EXERCISE TO WEEK
        public void AddExerciseToWeek()
        {
            Console.Clear();

            // Ensure there is at least one week
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks available. Add a week first.[/]");
                Console.ReadKey();
                return;
            }

            // Prompt user to select the week to add the exercise to
            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select week:[/]")
                    .AddChoices(loggedInUser.WeeklyPlans
                        .OrderBy(w => w.WeekNumber)
                        .Select(w => $"ID {w.Id} | Week {w.WeekNumber}").ToList())
            );

            int weekId = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());
            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Ensure there is at least one exercise
            if (!loggedInUser.Exercises.Any())
            {
                AnsiConsole.MarkupLine("[red]No exercises available. Add exercises first.[/]");
                Console.ReadKey();
                return;
            }

            // Prompt user to select an exercise
            var exChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select an exercise:[/]")
                    .AddChoices(loggedInUser.Exercises
                        .Select(e => $"ID {e.ID} | {e.Name}").ToList())
            );

            int exId = int.Parse(exChoice.Split('|')[0].Replace("ID", "").Trim());

            // Ask for day of week and time
            var day = AnsiConsole.Prompt(
                new SelectionPrompt<DayOfWeek>()
                    .Title("[yellow]Select day of the week:[/]")
                    .AddChoices(Enum.GetValues<DayOfWeek>())
            );

            var time = AnsiConsole.Ask<TimeSpan>("[yellow]Enter time of day (HH:mm):[/]");

            // Assign a new unique ID for the scheduled item
            int newId = selectedWeek.Items.Any() ? selectedWeek.Items.Max(i => i.Id) + 1 : 1;

            // Add the scheduled exercise to the week
            selectedWeek.Items.Add(new ScheduledItem
            {
                Id = newId,
                Day = day,
                TimeOfDay = time,
                ItemType = "Exercise",
                ItemId = exId
            });

            userDataService.SaveUser(loggedInUser);
            AnsiConsole.MarkupLine("[green]Exercise added to the week![/]");
            Console.ReadKey();
        }

        // ADD MEAL TO WEEK
        public void AddMealToWeek()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]ADD MEAL TO WEEK[/]");

            // Ensure at least one week exists
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks available. Add a week first.[/]");
                Console.ReadKey();
                return;
            }

            // Prompt user to select a week
            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select week:[/]")
                    .AddChoices(loggedInUser.WeeklyPlans
                        .OrderBy(w => w.WeekNumber)
                        .Select(w => $"ID {w.Id} | Week {w.WeekNumber}").ToList())
            );

            int weekId = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());
            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Load all meals directly from the JSON file
            var meals = mealManager.GetMealsFromFile();

            if (!meals.Any())
            {
                AnsiConsole.MarkupLine("[red]No meals available. Add meals first.[/]");
                Console.ReadKey();
                return;
            }

            // Prompt user to select a meal to schedule
            var mealChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a meal to schedule:[/]")
                    .AddChoices(meals
                        .Select(m => $"ID {m.Id} | {m.MealName}").ToList())
            );

            int mealId = int.Parse(mealChoice.Split('|')[0].Replace("ID", "").Trim());

            // Ask for day and time
            var day = AnsiConsole.Prompt(
                new SelectionPrompt<DayOfWeek>()
                    .Title("[yellow]Select day of the week:[/]")
                    .AddChoices(Enum.GetValues<DayOfWeek>())
            );

            var time = AnsiConsole.Ask<TimeSpan>("[yellow]Enter time of day (HH:mm):[/]");

            // Assign new ID for the scheduled item
            int newId = selectedWeek.Items.Any() ? selectedWeek.Items.Max(i => i.Id) + 1 : 1;

            // Add meal to schedule
            selectedWeek.Items.Add(new ScheduledItem
            {
                Id = newId,
                Day = day,
                TimeOfDay = time,
                ItemType = "Meal",
                ItemId = mealId
            });

            // Save changes
            userDataService.SaveUser(loggedInUser);
            AnsiConsole.MarkupLine("[green]Meal added to the week successfully![/]");
            Console.ReadKey();
        }

        // REMOVE EXERCISE OR MEAL FROM WEEK
        public void RemoveItemFromWeek()
        {
            Console.Clear();

            // Check if any weeks exist
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks available.[/]");
                Console.ReadKey();
                return;
            }

            // Week selection with a "Go Back" option
            var weekChoices = loggedInUser.WeeklyPlans
                .OrderBy(w => w.WeekNumber)
                .Select(w => $"ID {w.Id} | Week {w.WeekNumber}")
                .ToList();
            weekChoices.Add("Go Back"); // Add back option

            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select week to remove an item from:[/]")
                    .AddChoices(weekChoices)
            );

            if (weekChoice == "Go Back")
                return; // User canceled

            int weekId = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());
            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Check if any items exist
            if (!selectedWeek.Items.Any())
            {
                AnsiConsole.MarkupLine("[red]No items scheduled in this week.[/]");
                Console.ReadKey();
                return;
            }

            // Item selection with "Go Back" option
            var itemChoices = selectedWeek.Items.Select(i =>
            {
                string name;
                if (i.ItemType == "Exercise")
                {
                    var ex = loggedInUser.Exercises.FirstOrDefault(e => e.ID == i.ItemId);
                    name = ex != null ? ex.Name : "[Deleted Exercise]";
                }
                else
                {
                    var meal = mealManager.GetMealsFromFile().FirstOrDefault(m => m.Id == i.ItemId);
                    name = meal != null ? meal.MealName : "[Deleted Meal]";
                }

                return $"ID {i.Id} | {name} ({i.ItemType}, {i.Day} {i.TimeOfDay:hh\\:mm})";
            }).ToList();

            itemChoices.Add("Go Back"); // Add back option

            var itemChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select item to remove:[/]")
                    .AddChoices(itemChoices)
            );

            if (itemChoice == "Go Back")
                return; // User canceled

            // Parse and remove selected item
            int itemId = int.Parse(itemChoice.Split('|')[0].Replace("ID", "").Trim());
            selectedWeek.Items.RemoveAll(i => i.Id == itemId);

            // Save changes
            userDataService.SaveUser(loggedInUser);
            AnsiConsole.MarkupLine("[green]Item removed from the week![/]");
            Console.ReadKey();
        }

        // VIEW SINGLE WEEK
        public void ViewWeek()
        {
            Console.Clear();

            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks scheduled.[/]");
                Console.ReadKey();
                return;
            }

            // Select which week to view
            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select week to view:[/]")
                    .AddChoices(loggedInUser.WeeklyPlans
                        .OrderBy(w => w.WeekNumber)
                        .Select(w => $"ID {w.Id} | Week {w.WeekNumber}").ToList())
            );

            int weekId = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());
            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Display all scheduled items for this week
            AnsiConsole.MarkupLine($"[bold green]Week {selectedWeek.WeekNumber} Schedule:[/]");
            foreach (var item in selectedWeek.Items.OrderBy(i => i.Day).ThenBy(i => i.TimeOfDay))
            {
                string name = item.ItemType == "Exercise"
                    ? loggedInUser.Exercises.First(e => e.ID == item.ItemId).Name
                    : mealManager.GetMealsFromFile().FirstOrDefault(m => m.Id == item.ItemId)?.MealName ?? "[Deleted Meal]";

                AnsiConsole.MarkupLine($"{item.Day}, {item.TimeOfDay:hh\\:mm} - {name} ({item.ItemType})");
            }

            Console.ReadKey();
        }
    }
}
