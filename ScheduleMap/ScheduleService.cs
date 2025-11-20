using ShapeUp.Models;
using ShapeUp.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.ScheduleMap
{
    public class ScheduleService
    {
        private User loggedInUser;                  // The currently logged-in user
        private readonly UserDataService userDataService; // Helper to save/load user JSON

        // Constructor: store the logged-in user and prepare the JSON helper
        public ScheduleService(User user)
        {
            loggedInUser = user;                    // Store logged-in user reference
            userDataService = new UserDataService();// Prepare helper for saving/loading user data
        }

        // ADD NEW WEEK
        public void AddWeek()
        {
            Console.Clear();
            Console.WriteLine("Add New Week Schedule");

            // Ask for week number (1–52)
            Console.Write("Enter week number (1–52): ");
            if (!int.TryParse(Console.ReadLine(), out int weekNumber))
            {
                Console.WriteLine("Invalid week number.");
                Console.ReadKey();
                return; // Exit if input is invalid
            }

            // Generate unique ID for the new week
            int newWeekId = loggedInUser.WeeklyPlans.Any()
                ? loggedInUser.WeeklyPlans.Max(w => w.Id) + 1
                : 1;

            // Create new week object
            var newWeek = new Schedule
            {
                Id = newWeekId,           // Unique week ID
                WeekNumber = weekNumber,  // User-provided week number
                Exercises = new List<ScheduledExercise>() // Initialize empty list of scheduled exercises
            };

            // Add the week to the user's schedule list
            loggedInUser.WeeklyPlans.Add(newWeek);

            // Save updated user data back to JSON
            userDataService.SaveUser(loggedInUser);

            Console.WriteLine($"Week {weekNumber} added with ID {newWeekId}!");
            Console.ReadKey();
        }

        // VIEW ALL WEEKS
        public void ViewAllWeeks()
        {
            Console.Clear();

            // Check if there are any weeks
            if (!loggedInUser.WeeklyPlans.Any())
            {
                Console.WriteLine("No weeks scheduled yet.");
                Console.ReadKey();
                return;
            }

            // List all weeks with ID, week number, and number of exercises
            Console.WriteLine("All Scheduled Weeks:");
            foreach (var week in loggedInUser.WeeklyPlans.OrderBy(w => w.WeekNumber))
            {
                Console.WriteLine($"ID: {week.Id}, Week Number: {week.WeekNumber}, Exercises: {week.Exercises.Count}");
            }

            Console.ReadKey();
        }

        // DELETE A WEEK
        public void DeleteWeek()
        {
            Console.Clear();

            // Check if there are weeks to delete
            if (!loggedInUser.WeeklyPlans.Any())
            {
                Console.WriteLine("No weeks to delete.");
                Console.ReadKey();
                return;
            }

            // Display all weeks
            Console.WriteLine("Select week ID to delete:");
            foreach (var week in loggedInUser.WeeklyPlans.OrderBy(w => w.WeekNumber))
            {
                Console.WriteLine($"ID: {week.Id}, Week Number: {week.WeekNumber}");
            }

            // Ask user for week ID to delete
            if (!int.TryParse(Console.ReadLine(), out int id) || !loggedInUser.WeeklyPlans.Any(w => w.Id == id))
            {
                Console.WriteLine("Invalid week ID.");
                Console.ReadKey();
                return;
            }

            // Remove the week from the user's schedule
            loggedInUser.WeeklyPlans.RemoveAll(w => w.Id == id);

            // Save updated user data
            userDataService.SaveUser(loggedInUser);

            Console.WriteLine("Week deleted!");
            Console.ReadKey();
        }

        // ADD EXERCISE TO WEEK
        public void AddExerciseToWeek()
        {
            Console.Clear();

            // Check if any weeks exist
            if (!loggedInUser.WeeklyPlans.Any())
            {
                Console.WriteLine("No weeks available. Add a week first.");
                Console.ReadKey();
                return;
            }

            // Ask user to select a week
            Console.WriteLine("Select week ID:");
            foreach (var week in loggedInUser.WeeklyPlans.OrderBy(w => w.WeekNumber))
                Console.WriteLine($"ID: {week.Id}, Week Number: {week.WeekNumber}");

            if (!int.TryParse(Console.ReadLine(), out int weekId) || !loggedInUser.WeeklyPlans.Any(w => w.Id == weekId))
            {
                Console.WriteLine("Invalid week ID.");
                Console.ReadKey();
                return;
            }

            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Check if user has exercises to add
            if (!loggedInUser.Exercises.Any())
            {
                Console.WriteLine("No exercises available. Add exercises first.");
                Console.ReadKey();
                return;
            }

            // Show available exercises with their IDs
            Console.WriteLine("Select Exercise by ID:");
            foreach (var ex in loggedInUser.Exercises)
                Console.WriteLine($"ID: {ex.ID}, Name: {ex.Name}");

            if (!int.TryParse(Console.ReadLine(), out int exId) || !loggedInUser.Exercises.Any(e => e.ID == exId))
            {
                Console.WriteLine("Invalid exercise ID.");
                Console.ReadKey();
                return;
            }

            // Ask for the start date and time
            Console.Write("Enter start date and time (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startTime))
            {
                Console.WriteLine("Invalid date/time.");
                Console.ReadKey();
                return;
            }

            // Generate unique ID for the scheduled exercise in this week
            int newSchedId = selectedWeek.Exercises.Any() ? selectedWeek.Exercises.Max(e => e.Id) + 1 : 1;

            // Add scheduled exercise to the week
            selectedWeek.Exercises.Add(new ScheduledExercise
            {
                Id = newSchedId,       // Unique ID for this scheduled exercise
                ExerciseId = exId,     // Reference to the Exercise
                StartTime = startTime  // When the exercise is scheduled
            });

            // Save updated user data
            userDataService.SaveUser(loggedInUser);

            Console.WriteLine("Exercise added to week!");
            Console.ReadKey();
        }

        // REMOVE EXERCISE FROM WEEK
        public void RemoveExerciseFromWeek()
        {
            Console.Clear();

            // Check if any weeks exist
            if (!loggedInUser.WeeklyPlans.Any())
            {
                Console.WriteLine("No weeks available.");
                Console.ReadKey();
                return;
            }

            // Ask user to select a week
            Console.WriteLine("Select week ID to remove an exercise from:");
            foreach (var week in loggedInUser.WeeklyPlans.OrderBy(w => w.WeekNumber))
            {
                Console.WriteLine($"ID: {week.Id}, Week Number: {week.WeekNumber}");
            }

            if (!int.TryParse(Console.ReadLine(), out int weekId) || !loggedInUser.WeeklyPlans.Any(w => w.Id == weekId))
            {
                Console.WriteLine("Invalid week ID.");
                Console.ReadKey();
                return;
            }

            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Check if there are exercises scheduled in this week
            if (!selectedWeek.Exercises.Any())
            {
                Console.WriteLine("No exercises scheduled in this week.");
                Console.ReadKey();
                return;
            }

            // Show scheduled exercises with IDs and start times
            Console.WriteLine("Scheduled Exercises in this Week:");
            foreach (var sched in selectedWeek.Exercises)
            {
                var ex = loggedInUser.Exercises.First(e => e.ID == sched.ExerciseId);
                Console.WriteLine($"ID: {sched.Id}, {sched.StartTime:yyyy-MM-dd HH:mm} - {ex.Name} ({ex.DurationMinutes} min)");
            }

            // Ask user which scheduled exercise to remove
            Console.Write("Enter the ID of the scheduled exercise to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int schedId) || !selectedWeek.Exercises.Any(e => e.Id == schedId))
            {
                Console.WriteLine("Invalid scheduled exercise ID.");
                Console.ReadKey();
                return;
            }

            // Remove the scheduled exercise
            selectedWeek.Exercises.RemoveAll(e => e.Id == schedId);

            // Save updated user data
            userDataService.SaveUser(loggedInUser);

            Console.WriteLine("Scheduled exercise removed from the week!");
            Console.ReadKey();
        }

        // VIEW SINGLE WEEK
        public void ViewWeek()
        {
            Console.Clear();

            // Check if user has weeks scheduled
            if (!loggedInUser.WeeklyPlans.Any())
            {
                Console.WriteLine("No weeks scheduled.");
                Console.ReadKey();
                return;
            }

            // Ask user to select a week to view
            Console.WriteLine("Select week ID to view:");
            foreach (var week in loggedInUser.WeeklyPlans.OrderBy(w => w.WeekNumber))
                Console.WriteLine($"ID: {week.Id}, Week Number: {week.WeekNumber}");

            if (!int.TryParse(Console.ReadLine(), out int weekId) || !loggedInUser.WeeklyPlans.Any(w => w.Id == weekId))
            {
                Console.WriteLine("Invalid week ID.");
                Console.ReadKey();
                return;
            }

            var selectedWeek = loggedInUser.WeeklyPlans.First(w => w.Id == weekId);

            // Display all exercises in the week ordered by start time
            Console.WriteLine($"\nWeek {selectedWeek.WeekNumber} Schedule:");
            foreach (var sched in selectedWeek.Exercises.OrderBy(e => e.StartTime))
            {
                var ex = loggedInUser.Exercises.First(e => e.ID == sched.ExerciseId);
                Console.WriteLine($"{sched.StartTime:yyyy-MM-dd HH:mm} - {ex.Name} ({ex.DurationMinutes} min, {ex.MuscleGroup})");
            }

            Console.ReadKey();
        }
    }
}
