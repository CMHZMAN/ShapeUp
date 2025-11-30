using ShapeUp.Models;
using ShapeUp.Models.Exercises;
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
        private readonly UserDataService userDataService; // Helper to save/load user JSON

        // Constructor: store the logged-in user and prepare the JSON helper
        public ScheduleService(User user)
        {
            loggedInUser = user;                    // Store logged-in user reference
            userDataService = new UserDataService();// Prepare helper for saving/loading user data
        }

        // ADD NEW WEEK
        public void AddWeek()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]Add New Week Schedule[/]");

            // Ask for week number (1–52)
            int weekNumber = AnsiConsole.Ask<int>("[yellow]Enter week number (1–52):[/]");

            // Generate unique ID for the new week
            int newWeekId = loggedInUser.WeeklyPlans.Any()
                ? loggedInUser.WeeklyPlans.Max(w => w.Id) + 1
                : 1;

            // Create new week object
            var newWeek = new Schedule
            {
                Id = newWeekId,           // Unique week ID
                WeekNumber = weekNumber,  // User-provided week number
                Exercises = new List<ScheduledExercise>() // Initialize empty list of scheduled exercises
            };

            // Add the week to the user's schedule list
            loggedInUser.WeeklyPlans.Add(newWeek);

            // Save updated user data back to JSON
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine($"[green]Week {weekNumber} added with ID {newWeekId}![/]");
            Console.ReadKey();
        }

        // VIEW ALL WEEKS
        public void ViewAllWeeks()
        {
            Console.Clear();

            // Check if there are any weeks
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks scheduled yet.[/]");
                Console.ReadKey();
                return;
            }

            // List all weeks with ID, week number, and number of exercises
            AnsiConsole.MarkupLine("[bold green]All Scheduled Weeks:[/]");
            foreach (var week in loggedInUser.WeeklyPlans.OrderBy(w => w.WeekNumber))
            {
                AnsiConsole.MarkupLine($"[yellow]ID:[/] {week.Id}, Week Number: [cyan]{week.WeekNumber}[/], Exercises: [green]{week.Exercises.Count}[/]");
            }

            Console.ReadKey();
        }

        // DELETE A WEEK
        public void DeleteWeek()
        {
            Console.Clear();

            // Check if there are weeks to delete
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks to delete.[/]");
                Console.ReadKey();
                return;
            }

            // Ask user to select a week to delete using arrow keys
            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a week to delete:[/]")
                    .AddChoices(loggedInUser.WeeklyPlans
                        .OrderBy(w => w.WeekNumber)
                        .Select(w => $"ID {w.Id} | Week {w.WeekNumber}").ToList())
            );

            int id = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());

            // Remove the week from the user's schedule
            loggedInUser.WeeklyPlans.RemoveAll(w => w.Id == id);

            // Save updated user data
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine("[green]Week deleted![/]");
            Console.ReadKey();
        }

        // ADD EXERCISE TO WEEK
        public void AddExerciseToWeek()
        {
            Console.Clear();

            // Check if any weeks exist
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks available. Add a week first.[/]");
                Console.ReadKey();
                return;
            }

            // Ask user to select a week using arrow keys
            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select week:[/]")
                    .AddChoices(loggedInUser.WeeklyPlans
                        .OrderBy(w => w.WeekNumber)
                        .Select(w => $"ID {w.Id} | Week {w.WeekNumber}").ToList())
            );

            int weekId = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());
            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Check if user has exercises to add
            if (!loggedInUser.Exercises.Any())
            {
                AnsiConsole.MarkupLine("[red]No exercises available. Add exercises first.[/]");
                Console.ReadKey();
                return;
            }

            // Ask user to select an exercise using arrow keys
            var exChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select an exercise:[/]")
                    .AddChoices(loggedInUser.Exercises
                        .Select(e => $"ID {e.ID} | {e.Name}").ToList())
            );

            int exId = int.Parse(exChoice.Split('|')[0].Replace("ID", "").Trim());

            // Ask for the start date and time
            DateTime startTime = AnsiConsole.Ask<DateTime>("[yellow]Enter start date and time (yyyy-MM-dd HH:mm):[/]");

            // Generate unique ID for the scheduled exercise in this week
            int newSchedId = selectedWeek.Exercises.Any() ? selectedWeek.Exercises.Max(e => e.Id) + 1 : 1;

            // Add scheduled exercise to the week
            selectedWeek.Exercises.Add(new ScheduledExercise
            {
                Id = newSchedId,       // Unique ID for this scheduled exercise
                ExerciseId = exId,     // Reference to the Exercise
                StartTime = startTime  // When the exercise is scheduled
            });

            // Save updated user data
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine("[green]Exercise added to week![/]");
            Console.ReadKey();
        }

        // REMOVE EXERCISE FROM WEEK
        public void RemoveExerciseFromWeek()
        {
            Console.Clear();

            // Check if any weeks exist
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks available.[/]");
                Console.ReadKey();
                return;
            }

            // Ask user to select a week using arrow keys
            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select week to remove an exercise from:[/]")
                    .AddChoices(loggedInUser.WeeklyPlans
                        .OrderBy(w => w.WeekNumber)
                        .Select(w => $"ID {w.Id} | Week {w.WeekNumber}").ToList())
            );

            int weekId = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());
            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Check if there are exercises scheduled in this week
            if (!selectedWeek.Exercises.Any())
            {
                AnsiConsole.MarkupLine("[red]No exercises scheduled in this week.[/]");
                Console.ReadKey();
                return;
            }

            // Ask user to select the scheduled exercise to remove
            var schedChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select the scheduled exercise to remove:[/]")
                    .AddChoices(selectedWeek.Exercises
                        .Select(s => {
                            var ex = loggedInUser.Exercises.First(e => e.ID == s.ExerciseId);
                            return $"ID {s.Id} | {ex.Name} ({s.StartTime:yyyy-MM-dd HH:mm})";
                        }).ToList())
            );

            int schedId = int.Parse(schedChoice.Split('|')[0].Replace("ID", "").Trim());

            // Remove the scheduled exercise
            selectedWeek.Exercises.RemoveAll(e => e.Id == schedId);

            // Save updated user data
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine("[green]Scheduled exercise removed from the week![/]");
            Console.ReadKey();
        }

        // VIEW SINGLE WEEK
        public void ViewWeek()
        {
            Console.Clear();

            // Check if user has weeks scheduled
            if (!loggedInUser.WeeklyPlans.Any())
            {
                AnsiConsole.MarkupLine("[red]No weeks scheduled.[/]");
                Console.ReadKey();
                return;
            }

            // Ask user to select a week to view
            var weekChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select week to view:[/]")
                    .AddChoices(loggedInUser.WeeklyPlans
                        .OrderBy(w => w.WeekNumber)
                        .Select(w => $"ID {w.Id} | Week {w.WeekNumber}").ToList())
            );

            int weekId = int.Parse(weekChoice.Split('|')[0].Replace("ID", "").Trim());
            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Display all exercises in the week ordered by start time
            AnsiConsole.MarkupLine($"[bold green]Week {selectedWeek.WeekNumber} Schedule:[/]");
            foreach (var sched in selectedWeek.Exercises.OrderBy(e => e.StartTime))
            {
                var ex = loggedInUser.Exercises.First(e => e.ID == sched.ExerciseId);
                AnsiConsole.MarkupLine($"{sched.StartTime:yyyy-MM-dd HH:mm} - {ex.Name} ({ex.DurationMinutes} min, {ex.MuscleGroup})");
            }

            Console.ReadKey();
        }
    }
}
