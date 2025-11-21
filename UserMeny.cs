using ShapeUp.Models;
using ShapeUp.Models.Exercises;
using ShapeUp.ScheduleMap;
using ShapeUp.Models.Meal;
using ShapeUp.Profille;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp
{
        public class UserMenu
        {
            private readonly User loggedInUser;
            private readonly ExerciseService exerciseService;
            private readonly ScheduleService scheduleService;
            private readonly MealMenu2 mealMenu2;
            private readonly ProfilleMenu profilleMenu;

        public UserMenu(User user)
            {
                loggedInUser = user;
            }

            public void UserMe()
            {
                bool running = true;

                while (running)
                {
                    Console.Clear();
                    Console.WriteLine("User Menu");
                    Console.WriteLine("1. Exercises");
                    Console.WriteLine("2. Meal Plan");
                    Console.WriteLine("3. Exercises");
                    Console.WriteLine("4. Schedule");
                    Console.WriteLine("5. Profile");
                    Console.WriteLine("0. Log Out");
                    Console.Write("Choose: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                {
                    case "1":

                        break;
                    case "2":
                        mealMenu2.ShowMenu();
                        break;

                    case "3":
                        scheduleService.ViewAllWeeks();
                        break;

                    case "4":
                        scheduleService.AddExerciseToWeek();
                        break;
                    case"5":
                        profilleMenu.ShowMenu();
                        break;

                    case "0":
                            running = false;
                            Console.WriteLine("Logging out...");
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
