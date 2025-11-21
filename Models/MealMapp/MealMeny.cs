using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Meal
{
    public class MealMenu2
    {
        private readonly MealManager mealManager;
        public MealMenu2(int userId)
        {
            mealManager = new MealManager(userId);
        }
        public void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Meal Menu");
                Console.WriteLine("1. View Meals");
                Console.WriteLine("2. Add Meal");
                Console.WriteLine("3. Edit Meal");
                Console.WriteLine("4. Delete Meal");
                Console.WriteLine("0. Back to User Menu");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        mealManager.ShowAllMeal();
                        break;
                    case "2":
                        mealManager.AddMeal();
                        break;
                    case "3":
                        mealManager.EditMeal();
                        break;
                    case "4":
                        mealManager.DeleteMeal();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}