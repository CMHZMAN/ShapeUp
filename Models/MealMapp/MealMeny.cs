using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Meal
{
    public class MealMenu2
    {
        private readonly MealManager mealManager; // Hanterar alla meals för användaren

        public MealMenu2(int userId)
        {
            mealManager = new MealManager(userId); // Skapar MealManager för användaren
        }

        public void ShowMenu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                // Fancy Spectre.Console Title
                AnsiConsole.Markup("[bold yellow]Meal Menu[/]\n"); // Rubrik i gult och fet stil

                // Use Spectre.Console SelectionPrompt for arrow key navigation
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Choose an option:[/]") // Fråga i grönt
                        .HighlightStyle(new Style(foreground: Color.Gold1, decoration: Decoration.Bold)) // Markerad rad färg
                        .AddChoices(new[] {
                        "View Meals",    // 1
                        "Add Meal",      // 2
                        "Edit Meal",     // 3
                        "Delete Meal",   // 4
                        "Back to User Menu" // 0
                        })
                );

                // Hantera användarval
                switch (choice)
                {
                    case "View Meals":
                        mealManager.ShowAllMeal(); // Visa alla meals
                        break;
                    case "Add Meal":
                        mealManager.AddMeal(); // Lägg till ny meal
                        break;
                    case "Edit Meal":
                        mealManager.EditMeal(); // Redigera meal
                        break;
                    case "Delete Meal":
                        mealManager.DeleteMeal(); // Ta bort meal
                        break;
                    case "Back to User Menu":
                        running = false; // Gå tillbaka till användarmeny
                        break;
                }
            }
        }
    }
}