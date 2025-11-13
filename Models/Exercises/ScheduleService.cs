using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    internal class ScheduleService
    {
            private readonly User loggedInUser;
            private readonly UserDataService userDataService;

            public ScheduleService(User user)
            {
                loggedInUser = user;
                userDataService = new UserDataService();
            }

            public void AssignToSchedule()
            {
                Console.Clear();

                if (loggedInUser.Exercises.Count == 0)
                {
                    Console.WriteLine("Add an exercise first.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Choose an exercise:");
                for (int i = 0; i < loggedInUser.Exercises.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {loggedInUser.Exercises[i].Name}");
                }

                if (!int.TryParse(Console.ReadLine(), out int exIndex) ||
                    exIndex < 1 || exIndex > loggedInUser.Exercises.Count)
                {
                    Console.WriteLine("Invalid choice!");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter day of week (e.g., Monday): ");
                string day = Console.ReadLine();

                loggedInUser.WeeklySchedule[day] = loggedInUser.Exercises[exIndex - 1].Name;
                userDataService.SaveUser(loggedInUser);

                Console.WriteLine($"Assigned {loggedInUser.Exercises[exIndex - 1].Name} to {day}!");
                Console.ReadKey();
            }

            public void ViewSchedule()
            {
                Console.Clear();

                if (loggedInUser.WeeklySchedule.Count == 0)
                {
                    Console.WriteLine("No schedule yet.");
                }
                else
                {
                    Console.WriteLine("=== Weekly Schedule ===");
                    foreach (var day in loggedInUser.WeeklySchedule)
                    {
                        Console.WriteLine($"{day.Key}: {day.Value}");
                    }
                }

                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }
        }
    }

