using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    internal class ExerciseService
    {
            private readonly User loggedInUser;
            private readonly UserDataService userDataService;

            public ExerciseService(User user)
            {
                loggedInUser = user;
                userDataService = new UserDataService();
            }

            public void ViewExercises()
            {
                Console.Clear();

                if (loggedInUser.Exercises.Count == 0)
                {
                    Console.WriteLine("No exercises yet.");
                }
                else
                {
                    Console.WriteLine("Your Exercises:");
                    foreach (var ex in loggedInUser.Exercises)
                    {
                        Console.WriteLine($"- {ex.Name} ({ex.DurationMinutes} min)");
                    }
                }

                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }

            public void AddExercise()
            {
                Console.Clear();
                Console.Write("Enter exercise name: ");
                string name = Console.ReadLine();

                Console.Write("Enter duration in minutes: ");
                if (!int.TryParse(Console.ReadLine(), out int duration))
                {
                    Console.WriteLine("Invalid input!");
                    Console.ReadKey();
                    return;
                }

                var newExercise = new Exercise { Name = name, DurationMinutes = duration };
                loggedInUser.Exercises.Add(newExercise);

                userDataService.SaveUser(loggedInUser);
                Console.WriteLine($"{name} added!");
                Console.ReadKey();
            }
        }
    }

