using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

namespace ShapeUp.Models.Meal
{


    public class MealManager
    {
        private readonly string filePath; //söker i json filen baserat på user id
        private List<Meal> meals; //Lista av meals

        public MealManager(int id)
        {
            filePath = $"meals_{id}.json";
            meals = LoadMeals();
        }

        public void AddMeal()// Lägger till meal
        {
            Console.Clear();
            Console.WriteLine("ADD NEW MEAL");

            Console.Write("Enter meal name: ");
            string mealName = Console.ReadLine();

            List<Ingredient> ingredients = GetIngredient(); // Hämtar ingredienser från användaren

            meals.Add(new Meal // Skapar en ny meal och lägger till i listan
            {
                MealName = mealName,
                Ingredients = ingredients,
                TotalCalories = ingredients.Sum(i => i.Calories)
            });
            SaveMeal();

            Console.WriteLine("Meal added successfully!");


        }



        private List<Ingredient> GetIngredient()
        {
            var ingredients = new List<Ingredient>();
            Console.WriteLine("Enter ingredients (type 'done' when finished):");

            while (true)
            {
                Console.Write("Ingredient name: ");
                string name = Console.ReadLine();


                if (name.ToLower() == "done") break;
                Console.Write("Calories: ");
                if (int.TryParse(Console.ReadLine(), out int calories))
                {
                }
                else
                {
                    Console.WriteLine("Invalid calorie input. Please enter a number.");
                    continue;
                }

                ingredients.Add(new Ingredient { Name = name, Calories = calories });
            }
            return ingredients;
        }

        public void ShowAllMeal()
        {
            Console.Clear();
            if (meals.Count == 0)
            {
                Console.WriteLine("No meals available.");
                return;
            }
            Console.WriteLine("ALL MEALS:");
            int index = 1;
            foreach (var meal in meals) // Loopar igenom alla meals i listan
            {
                Console.WriteLine($"{index}. {meal.MealName} - {meal.TotalCalories} calories");
                index++;
            }
        }
        public void EditMeal()
        {
            Console.Clear();

            if (meals.Count == 0)
            {
                Console.WriteLine("No meals available to edit.");
                return;
            }

            ShowAllMeal();

            Console.WriteLine("Choose a number to edit");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > meals.Count)
            {
                Console.WriteLine("Try a number ");
                return;
            }
            Meal meal = meals[index - 1]; // Hämtar meal baserat på användarens val

            Console.WriteLine("Redigerar: {meal.MealName}");
            Console.Write("Enter new meal name '{meal.MealName}'):");
            string newName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newName))
            {
                meal.MealName = newName; //uppdaterar meal namn

                Console.WriteLine("Update ingredients:");
                List<Ingredient> newIngredients = GetIngredient();
                if (newIngredients.Count > 0) // om nya ingredienser har lagts till
                {
                    meal.Ingredients = newIngredients;
                    meal.TotalCalories = newIngredients.Sum(i => i.Calories);

                }
                SaveMeal();
                Console.WriteLine("Meal updated ");
            }
        }


        public void DeleteMeal()
        {
            Console.Clear();
            if (meals.Count == 0)
            {
                Console.WriteLine("No meals available to delete.");
                return;
            }
            ShowAllMeal();
            Console.WriteLine("Choose a number to delete");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > meals.Count)
            {
                Console.WriteLine("Try a number ");
                return;
            }
            meals.RemoveAt(index - 1); // Tar bort meal från listan
            SaveMeal();
            Console.WriteLine("Meal deleted successfully!");
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