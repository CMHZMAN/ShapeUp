using ShapeUp.Models;
using ShapeUp.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.UserMenu
{
    public class UserMenu
    {
        // Declare fields for all the menu classes
        private Exercise exercise;
        private MealAdviceAI mealAdviceAI;
        private NutritionGoal nutritionGoal;
        private ProgressReport progressReport;
        private WorkoutSession workoutSession;

        // Constructor: initializes all menu classes when MainMenu is created
        public UserMenu()
        {
            exercise = new Exercise();               // Initialize Exercise menu
            mealAdviceAI = new MealAdviceAI();       // Initialize Meal Advice AI menu
            nutritionGoal = new NutritionGoal();     // Initialize Nutrition Goals menu
            progressReport = new ProgressReport();   // Initialize Progress Report menu
            workoutSession = new WorkoutSession();   // Initialize Workout Sessions menu
        }

        // Main loop for the menu
        public void UserMe()
        {
            bool running = true; // Controls the menu loop

            while (running) // Loop until user chooses to log out
            {
                Console.Clear(); // Clear the console for a fresh menu display

                // Display the main menu options
                Console.WriteLine("User Menu");
                Console.WriteLine("1. Exercises");
                Console.WriteLine("2. Meal Advice AI");
                Console.WriteLine("3. Nutrition Goals");
                Console.WriteLine("4. Progress Report");
                Console.WriteLine("5. Workout Sessions");
                Console.WriteLine("0. Log Out");
                Console.Write("Choose: ");

                string choice = Console.ReadLine(); // Read user's input

                // Use a switch statement to handle menu selection
                switch (choice)
                {
                    case "1":
                        // Behöver fyllas i med väg till nästa klass/Meny/ETC // Call the Exercise menu
                        break;

                    case "2":
                        // Behöver fyllas i med väg till nästa klass/Meny/ETC // Call the Meal Advice AI menu
                        break;

                    case "3":
                        // Behöver fyllas i med väg till nästa klass/Meny/ETC // Call the Nutrition Goals menu
                        break;

                    case "4":
                        // Behöver fyllas i med väg till nästa klass/Meny/ETC // Call the Progress Report menu
                        break;

                    case "5":
                        // Behöver fyllas i med väg till nästa klass/Meny/ETC  // Call the Workout Sessions menu
                        break;

                    case "0":
                        Console.WriteLine("Logging out..."); // Inform user they are logging out
                        running = false; // Stop the menu loop
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again."); // Handle invalid input
                        break;
                }

                // After a submenu is executed or invalid input, loop continues automatically
                // The console will refresh and display the main menu again
            }
        }
    }
    
}
