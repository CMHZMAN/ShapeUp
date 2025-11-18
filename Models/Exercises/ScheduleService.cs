using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    internal class ScheduleService
    {
        private readonly User loggedInUser;            // The user currently logged in
        private readonly UserDataService userDataService; // Handles saving/loading user JSON data

        // Constructor — runs when ScheduleService is created
        public ScheduleService(User user)
        {
            loggedInUser = user;                       // Store the logged-in user
            userDataService = new UserDataService();   // Prepare JSON helper for saving updates
        }


        // ASSIGN AN EXERCISE TO A DAY
        public void AssignToSchedule()
        {
            Console.Clear();

            // If the user has no exercises, they cannot schedule anything
            if (loggedInUser.Exercises.Count == 0)
            {
                Console.WriteLine("You must add an exercise before assigning it.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Choose an exercise to schedule:\n");

            // List all exercises so user can choose one
            for (int i = 0; i < loggedInUser.Exercises.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {loggedInUser.Exercises[i].Name}");
            }

            // Validate that the user picks a correct exercise number
            if (!int.TryParse(Console.ReadLine(), out int exIndex) ||
                exIndex < 1 || exIndex > loggedInUser.Exercises.Count)
            {
                Console.WriteLine("Invalid selection.");
                Console.ReadKey();
                return;
            }

            // Ask for what day the exercise should be assigned to
            Console.Write("\nEnter the day of the week (e.g., Monday): ");
            string day = Console.ReadLine();

            // Save the exercise name to the dictionary representing the weekly schedule
            loggedInUser.WeeklySchedule[day] = loggedInUser.Exercises[exIndex - 1].Name;

            // Save changes to JSON file
            userDataService.SaveUser(loggedInUser);

            Console.WriteLine($"\nAssigned '{loggedInUser.Exercises[exIndex - 1].Name}' to {day}.");
            Console.ReadKey();
        }


        // VIEW WEEKLY SCHEDULE
        public void ViewSchedule()
        {
            Console.Clear();

            // If schedule is empty, tell the user
            if (loggedInUser.WeeklySchedule.Count == 0)
            {
                Console.WriteLine("Your weekly schedule is empty.\nAdd exercises to it first.");
            }
            else
            {
                Console.WriteLine("=== WEEKLY SCHEDULE ===\n");

                // Loop through all scheduled days and show them
                foreach (var day in loggedInUser.WeeklySchedule)
                {
                    Console.WriteLine($"{day.Key}: {day.Value}");
                }
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}

