using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShapeUp.Models.Meal
{
    public class MealManager
    {
        private readonly string filePath; // söker i json filen baserat på user id
        private List<Meal> meals; // Lista av meals
        public List<Meal> GetMeals()
        {
            return meals;
        }

        public MealManager(int id)
        {
            filePath = $"meals_{id}.json";
            meals = LoadMeals();
        }

        private List<Meal> LoadMeals()
        {
            if (!File.Exists(filePath))
                return new List<Meal>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
        }

        public void AddMeal() // Lägger till meal
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]ADD NEW MEAL[/]");

            string mealName = AnsiConsole.Ask<string>("[yellow]Enter meal name:[/]");

            List<Ingredient> ingredients = GetIngredient(); // Hämtar ingredienser från användaren

            meals.Add(new Meal // Skapar en ny meal och lägger till i listan
            {
                MealName = mealName,
                Ingredients = ingredients,
                TotalCalories = ingredients.Sum(i => i.Calories)
            });

            SaveMeal();
            AnsiConsole.MarkupLine("[green]Meal added successfully![/]");
            Console.ReadKey();
        }

        private List<Ingredient> GetIngredient()
        {
            var ingredients = new List<Ingredient>();
            AnsiConsole.MarkupLine("[yellow]Enter ingredients (type 'done' at any time to finish):[/]");

            while (true)
            {
                // Ask for ingredient name
                string name = AnsiConsole.Ask<string>("[yellow]Ingredient name:[/]");

                if (string.Equals(name, "done", StringComparison.OrdinalIgnoreCase))
                    break; // Exit loop if done

                // Ask for calories with validation
                int calories;
                while (true)
                {
                    string calInput = AnsiConsole.Ask<string>("[yellow]Calories:[/]");
                    if (string.Equals(calInput, "done", StringComparison.OrdinalIgnoreCase))
                    {
                        // User typed "done" instead of a number → exit ingredient input
                        goto FinishIngredients;
                    }
                    if (int.TryParse(calInput, out calories))
                        break; // Valid number entered
                    AnsiConsole.MarkupLine("[red]Invalid number. Please enter calories or type 'done' to finish.[/]");
                }

                ingredients.Add(new Ingredient { Name = name, Calories = calories });
            }

FinishIngredients:
            return ingredients;
        }

        public void ShowAllMeal()
        {
            Console.Clear();

            if (meals.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No meals available.[/]");
                Console.ReadKey();
                return;
            }

            AnsiConsole.MarkupLine("[bold green]ALL MEALS:[/]");
            int index = 1;
            foreach (var meal in meals) // Loopar igenom alla meals i listan
            {
                AnsiConsole.MarkupLine($"[yellow]{index}.[/] [cyan]{meal.MealName}[/] - [green]{meal.TotalCalories}[/] calories");
                index++;
            }

            Console.ReadKey();
        }

        public void EditMeal()
        {
            Console.Clear();

            if (meals.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No meals available to edit.[/]");
                Console.ReadKey();
                return;
            }

            // Ask user to select a meal using arrow keys
            var mealChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a meal to edit:[/]")
                    .AddChoices(meals.Select((m, i) => $"{i + 1} | {m.MealName}").ToList())
            );

            int index = int.Parse(mealChoice.Split('|')[0].Trim()) - 1;
            Meal meal = meals[index]; // Hämtar meal baserat på användarens val

            Console.Clear();
            AnsiConsole.MarkupLine($"[bold green]Editing: {meal.MealName}[/]");

            string newName = AnsiConsole.Ask<string>($"New meal name (leave blank to keep '{meal.MealName}'):");
            List<Ingredient> newIngredients = GetIngredient();

            if (!string.IsNullOrWhiteSpace(newName))
                meal.MealName = newName; // uppdaterar meal namn

            if (newIngredients.Count > 0)
            {
                meal.Ingredients = newIngredients;
                meal.TotalCalories = newIngredients.Sum(i => i.Calories);
            }

            SaveMeal();
            AnsiConsole.MarkupLine("[green]Meal updated![/]");
            Console.ReadKey();
        }

        public void DeleteMeal()
        {
            Console.Clear();

            if (meals.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No meals to delete.[/]");
                Console.ReadKey();
                return;
            }

            // Ask user to select a meal to delete, add "Go Back" at the bottom
            var mealChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a meal to delete:[/]")
                    .AddChoices(meals
                        .Select((m, i) => $"{i + 1} | {m.MealName}")
                        .Concat(new[] { "0 | Go Back" }) // <-- Go Back option
                        .ToList())
            );

            // If user chose Go Back, just return
            if (mealChoice.StartsWith("0"))
                return;

            int index = int.Parse(mealChoice.Split('|')[0].Trim()) - 1;
            var meal = meals[index];

            // Escape special markup characters
            string safeMealName = meal.MealName.Replace("[", "\\[").Replace("]", "\\]");

            // Confirm deletion
            string confirm = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Are you sure you want to delete the meal: [cyan]{safeMealName}[/]?")
                    .AddChoices(new[] { "Yes", "No" })
            );

            if (confirm != "Yes")
            {
                AnsiConsole.MarkupLine("[yellow]Deletion canceled.[/]");
                Console.ReadKey();
                return;
            }

            // Remove the meal from the list
            meals.RemoveAt(index);
            SaveMeal();

            AnsiConsole.MarkupLine("[green]Meal deleted successfully![/]");
            Console.ReadKey();
        }

        private void SaveMeal()
        {
            string json = JsonSerializer.Serialize(meals, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Get meals from file without loading into the manager
        public List<Meal> GetMealsFromFile()
        {
            if (!File.Exists(filePath))
                return new List<Meal>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
        }
    }
}