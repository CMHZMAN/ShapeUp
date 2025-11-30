using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    public class ExerciseService
    {
        // Fields (data stored inside the service)
        private User loggedInUser;
        // The user who is currently logged in.
        // All exercises created/edited belong to THIS user only.

        private readonly UserDataService userDataService;
        // This handles saving the updated user data back into user.json.

        // Runs when ExerciseService is created from UserMenu
        public ExerciseService(User user)
        {
            loggedInUser = user;                  // Store the logged-in user
            userDataService = new UserDataService(); // Prepare JSON saving/loading helper
        }

        // Shows all exercises saved by this user
        public void ViewExercises()
        {
            Console.Clear();

            // Check if user has created any exercises yet
            if (loggedInUser.Exercises.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]You have no exercises yet.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold green]Your Exercises[/]\n");

                // Loop through each exercise
                foreach (var ex in loggedInUser.Exercises)
                {
                    // Display all exercise fields, including the ID
                    AnsiConsole.MarkupLine($"[yellow]ID:[/] {ex.ID} | [cyan]{ex.Name}[/]");
                    AnsiConsole.MarkupLine($"   Duration: [green]{ex.DurationMinutes}[/] min");
                    AnsiConsole.MarkupLine($"   Description: [green]{ex.Description}[/]");
                    AnsiConsole.MarkupLine($"   Muscle Group: [green]{ex.MuscleGroup}[/]");
                    AnsiConsole.MarkupLine($"   Difficulty: [green]{ex.Difficulty}[/]");
                    Console.WriteLine();
                }
            }

            // Pause so user can see the list
            AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
            Console.ReadKey();
        }

        // Creates a new exercise and saves it to the user's profile
        public void AddExercise()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]Add New Exercise[/]");

            // Ask for basic exercise name
            string name = AnsiConsole.Ask<string>("[yellow]Enter exercise name:[/]");

            // Ask for duration (must be a number)
            int duration = AnsiConsole.Ask<int>("[yellow]Enter duration in minutes:[/]");

            // Ask for description of the exercise
            string description = AnsiConsole.Ask<string>("[yellow]Enter description:[/]");

            // USER SELECTS MUSCLE GROUP
            string muscleGroup = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Choose muscle group:[/]")
                    .AddChoices(new[] { "Legs", "Chest", "Back", "Arms", "Core" })
            );

            // USER SELECTS DIFFICULTY LEVEL
            string difficulty = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Choose difficulty:[/]")
                    .AddChoices(new[] { "Easy", "Medium", "Hard" })
            );

            // Generate a new unique ID for this exercise
            int newId = loggedInUser.Exercises.Any()
                ? loggedInUser.Exercises.Max(e => e.ID) + 1
                : 1;

            // CREATE THE EXERCISE OBJECT
            var newExercise = new Exercise
            {
                ID = newId,                        // Assign the generated ID
                Name = name,
                DurationMinutes = duration,
                Description = description,
                MuscleGroup = muscleGroup,
                Difficulty = difficulty
            };

            // Add new exercise to logged-in user's exercise list
            loggedInUser.Exercises.Add(newExercise);

            // Save updated user data back to JSON
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine("[green]Exercise added![/]");
            Console.ReadKey();
        }

        // Allows editing name, duration, muscle, difficulty, description
        public void EditExercise()
        {
            Console.Clear();

            // Check if user has at least one exercise
            if (loggedInUser.Exercises.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No exercises to edit.[/]");
                Console.ReadKey();
                return;
            }

            // List exercises so user can choose which one to edit
            var choices = loggedInUser.Exercises
                .Select(e => $"{e.ID} | {e.Name}")
                .ToList();

            string selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select an exercise to edit:[/]")
                    .AddChoices(choices)
            );

            int id = int.Parse(selected.Split('|')[0].Trim());

            // Get the selected exercise by ID
            var exToEdit = loggedInUser.Exercises.First(e => e.ID == id);

            Console.Clear();
            AnsiConsole.MarkupLine($"[bold green]Editing '{exToEdit.Name}'[/]\n");

            // Ask for new values (optional)
            string newName = AnsiConsole.Ask<string>($"New name (leave blank to keep '{exToEdit.Name}'):");
            string newDurationInput = AnsiConsole.Ask<string>($"New duration (current {exToEdit.DurationMinutes}):");
            string newDescription = AnsiConsole.Ask<string>("New description (leave empty to keep current):");

            // MUSCLE GROUP
            string newMG = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]New muscle group (leave blank to keep current):[/]")
                    .AddChoices(new[] { "", "Legs", "Chest", "Back", "Arms", "Core" })
            );

            // DIFFICULTY LEVEL
            string newDiff = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]New difficulty (leave blank to keep current):[/]")
                    .AddChoices(new[] { "", "Easy", "Medium", "Hard" })
            );

            // APPLY CHANGES IF USER ENTERED NEW VALUES
            if (!string.IsNullOrWhiteSpace(newName))
                exToEdit.Name = newName;

            if (int.TryParse(newDurationInput, out int newDuration))
                exToEdit.DurationMinutes = newDuration;

            if (!string.IsNullOrWhiteSpace(newDescription))
                exToEdit.Description = newDescription;

            if (!string.IsNullOrWhiteSpace(newMG))
                exToEdit.MuscleGroup = newMG;

            if (!string.IsNullOrWhiteSpace(newDiff))
                exToEdit.Difficulty = newDiff;

            // Save updated data
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine("[green]Exercise updated![/]");
            Console.ReadKey();
        }

        // Delete an exercise
        public void DeleteExercise()
        {
            Console.Clear();

            // Check if the user has ANY exercises
            if (loggedInUser.Exercises.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]You have no exercises to delete.[/]");
                Console.ReadKey();
                return;
            }

            // Show exercises so user can choose what to delete
            var choices = loggedInUser.Exercises
                .Select(e => $"{e.ID} | {e.Name}")
                .ToList();

            string selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select an exercise to delete:[/]")
                    .AddChoices(choices)
            );

            int id = int.Parse(selected.Split('|')[0].Trim());

            // Get the selected exercise by ID
            var exToDelete = loggedInUser.Exercises.First(e => e.ID == id);

            // Confirm deletion
            string confirm = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Are you sure you want to delete '{exToDelete.Name}'?[/]")
                    .AddChoices(new[] { "y", "n" })
            );

            if (confirm != "y")
            {
                AnsiConsole.MarkupLine("[yellow]Deletion canceled.[/]");
                Console.ReadKey();
                return;
            }

            // Remove the exercise from the list
            loggedInUser.Exercises.Remove(exToDelete);

            // Save changes to JSON
            userDataService.SaveUser(loggedInUser);

            AnsiConsole.MarkupLine("[green]Exercise deleted successfully![/]");
            Console.ReadKey();
        }
    }
}

