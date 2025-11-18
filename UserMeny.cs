using ShapeUp.Models;
using ShapeUp.Models.Exercises;
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
                    Console.WriteLine("0. Log Out");
                    Console.Write("Choose: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            // Pass the logged-in user to the ExerciseMenu
                            ExerciseMenu exerciseMenu = new ExerciseMenu(loggedInUser);
                            exerciseMenu.ShowMenu();
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
