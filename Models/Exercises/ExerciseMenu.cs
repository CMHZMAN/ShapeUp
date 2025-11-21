using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    public class ExerciseMenu
    {
        private readonly User loggedInUser;
        private readonly ExerciseService exerciseService;

        public ExerciseMenu(User user)
        {
            loggedInUser = user;
            exerciseService = new ExerciseService(loggedInUser);
        }

        public void ShowMenu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Exercise Menu");
                Console.WriteLine("1. View Exercises");
                Console.WriteLine("2. Add Exercise");
                Console.WriteLine("0. Back to User Menu");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        exerciseService.ViewExercises();
                        break;
                    case "2":
                        exerciseService.AddExercise();
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

