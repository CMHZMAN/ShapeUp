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

        public MealManager(int id)
        {
            filePath = $"meals_{id}.json";
            meals = LoadMeals();
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
            AnsiConsole.MarkupLine("[yellow]Enter ingredients (type 'done' to finish):[/]");

            while (true)
            {
                string name = AnsiConsole.Ask<string>("[yellow]Ingredient name:[/]");

                if (name.ToLower() == "done") break;

                int calories = AnsiConsole.Ask<int>("[yellow]Calories:[/]");

                ingredients.Add(new Ingredient { Name = name, Calories = calories });
            }

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

            // Ask user to select a meal to delete using arrow keys
            var mealChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a meal to delete:[/]")
                    .AddChoices(meals.Select((m, i) => $"{i + 1} | {m.MealName}").ToList())
            );

            int index = int.Parse(mealChoice.Split('|')[0].Trim()) - 1;
            Meal meal = meals[index]; // Tar bort meal från listan

            // Confirm deletion
            string confirm = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Are you sure you want to delete '{meal.MealName}'?[/]")
                    .AddChoices(new[] { "y", "n" })
            );

            if (confirm != "y")
            {
                AnsiConsole.MarkupLine("[yellow]Deletion canceled.[/]");
                Console.ReadKey();
                return;
            }

            meals.RemoveAt(index); // Tar bort meal från listan
            SaveMeal();
            AnsiConsole.MarkupLine("[green]Meal deleted![/]");
            Console.ReadKey();
        }

        private List<Meal> LoadMeals()
        {
            if (!File.Exists(filePath))
                return new List<Meal>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
        }

        private void SaveMeal()
        {
            string json = JsonSerializer.Serialize(meals, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}