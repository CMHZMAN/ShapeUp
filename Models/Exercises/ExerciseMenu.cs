using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    internal class ExerciseMenu
    {
            private readonly User loggedInUser;
            private readonly ExerciseService exerciseService;
            private readonly ScheduleService scheduleService;

            public ExerciseMenu(User user)
            {
                loggedInUser = user;
                exerciseService = new ExerciseService(user);
                scheduleService = new ScheduleService(user);
            }

            public void ShowMenu()
            {
                bool running = true;

                while (running)
                {
                    Console.Clear();
                    Console.WriteLine("EXERCISE MENU");
                    Console.WriteLine("1. View Exercises");
                    Console.WriteLine("2. Add New Exercise");
                    Console.WriteLine("3. Assign Exercise to Weekly Plan");
                    Console.WriteLine("4. View Weekly Schedule");
                    Console.WriteLine("0. Back to Main Menu");
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

                        case "3":
                            scheduleService.AssignToSchedule();
                            break;

                        case "4":
                            scheduleService.ViewSchedule();
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

