using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    public class ExerciseMenu
    {
        private readonly User loggedInUser;  // The logged-in user
        private readonly ExerciseService exerciseService; // Handles exercise CRUD

        public ExerciseMenu(User user)
        {
            loggedInUser = user;
            exerciseService = new ExerciseService(loggedInUser); // Initialize service with current user
        }

        public void ShowMenu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                // Fancy Spectre.Console title
                AnsiConsole.MarkupLine("[bold yellow]Exercise Menu[/]"); // Menu title

                // Create arrow-key menu using Spectre.Console
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Choose an option:[/]")
                        .HighlightStyle(new Style(foreground: Color.Gold1, decoration: Decoration.Bold)) // Highlight style
                        .AddChoices(new[]
                        {
                        "View Exercises", // Option 1
                        "Add Exercise",   // Option 2
                        "Edit Exercise",  // Option 3
                        "Delete Exercise",// Option 4
                        "Back to User Menu" // Option 5
                        })
                );

                // Menu actions based on selection
                switch (choice)
                {
                    case "View Exercises":
                        exerciseService.ViewExercises(); // Show all exercises
                        break;
                    case "Add Exercise":
                        exerciseService.AddExercise(); // Add new exercise
                        break;
                    case "Edit Exercise":
                        exerciseService.EditExercise(); // Edit selected exercise
                        break;
                    case "Delete Exercise":
                        exerciseService.DeleteExercise(); // Delete selected exercise
                        break;
                    case "Back to User Menu":
                        running = false; // Exit menu
                        break;
                }
            }
        }
    }

}

