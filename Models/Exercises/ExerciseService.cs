using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    public class ExerciseService
    {
        // Fields (data stored inside the service)

        private User loggedInUser;
        // The user who is currently logged in.
        // All exercises created/edited belong to THIS user only.

        private readonly UserDataService userDataService;
        // This handles saving the updated user data back into user.json.

        // Runs when ExerciseService is created from UserMenu
        public ExerciseService(User user)
        {
            loggedInUser = user;                  // Store the logged-in user
            userDataService = new UserDataService(); // Prepare JSON saving/loading helper
        }

        // Shows all exercises saved by this user
        public void ViewExercises()
        {
            Console.Clear();

            // Check if user has created any exercises yet
            if (loggedInUser.Exercises.Count == 0)
            {
                Console.WriteLine("You have no exercises yet.");
            }
            else
            {
                Console.WriteLine("Your Exercises");

                // Loop through each exercise
                for (int i = 0; i < loggedInUser.Exercises.Count; i++)
                {
                    var ex = loggedInUser.Exercises[i];  // The current exercise

                    // Display all exercise fields, including the ID
                    Console.WriteLine($"ID: {ex.ID} | {ex.Name}");
                    Console.WriteLine($"   Duration: {ex.DurationMinutes} min");
                    Console.WriteLine($"   Description: {ex.Description}");
                    Console.WriteLine($"   Muscle Group: {ex.MuscleGroup}");
                    Console.WriteLine($"   Difficulty: {ex.Difficulty}");
                    Console.WriteLine();
                }
            }

            // Pause so user can see the list
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        // Creates a new exercise and saves it to the user's profile
        public void AddExercise()
        {
            Console.Clear();
            Console.WriteLine("Add New Exercise");

            // Ask for basic exercise name
            Console.Write("Enter exercise name: ");
            string name = Console.ReadLine();

            // Ask for duration (must be a number)
            Console.Write("Enter duration in minutes: ");
            if (!int.TryParse(Console.ReadLine(), out int duration))
            {
                Console.WriteLine("Invalid duration.");
                Console.ReadKey();
                return;  // Stop if duration is not a number
            }

            // Ask for description of the exercise
            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            // USER SELECTS MUSCLE GROUP
            Console.WriteLine("Choose muscle group:");
            Console.WriteLine("1. Legs");
            Console.WriteLine("2. Chest");
            Console.WriteLine("3. Back");
            Console.WriteLine("4. Arms");
            Console.WriteLine("5. Core");
            Console.Write("Choose: ");
            string mgChoice = Console.ReadLine();

            // Convert number -> text
            string muscleGroup = mgChoice switch
            {
                "1" => "Legs",
                "2" => "Chest",
                "3" => "Back",
                "4" => "Arms",
                "5" => "Core",
                _ => "General"    // If invalid choice, default to "General"
            };

            // USER SELECTS DIFFICULTY LEVEL
            Console.WriteLine("Choose difficulty:");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");
            Console.Write("Choose: ");
            string diffChoice = Console.ReadLine();

            // Convert number -> text
            string difficulty = diffChoice switch
            {
                "1" => "Easy",
                "2" => "Medium",
                "3" => "Hard",
                _ => "Medium"     // Default if invalid input
            };

            // ============================
            // Generate unique ID for the new exercise
            // ============================
            int newID = loggedInUser.Exercises.Count == 0
                ? 1 // If no exercises yet, start with ID 1
                : loggedInUser.Exercises.Max(e => e.ID) + 1; // Otherwise, increment max ID

            // CREATE THE EXERCISE OBJECT
            var newExercise = new Exercise
            {
                ID = newID,                         // Assign the unique ID
                Name = name,
                DurationMinutes = duration,
                Description = description,
                MuscleGroup = muscleGroup,
                Difficulty = difficulty
            };

            // Add new exercise to logged-in user's exercise list
            loggedInUser.Exercises.Add(newExercise);

            // Save updated user data back to JSON
            userDataService.SaveUser(loggedInUser);

            Console.WriteLine($"Exercise added with ID {newID}!");
            Console.ReadKey();
        }

        // Allows editing name, duration, muscle, difficulty, description
        public void EditExercise()
        {
            Console.Clear();

            // Check if user has at least one exercise
            if (loggedInUser.Exercises.Count == 0)
            {
                Console.WriteLine("No exercises to edit.");
                Console.ReadKey();
                return;
            }

            // List exercises so user can choose which one to edit
            Console.WriteLine("Which exercise do you want to edit?");
            foreach (var ex in loggedInUser.Exercises)
            {
                Console.WriteLine($"ID: {ex.ID} | {ex.Name}");
            }

            // Validate numeric choice
            Console.Write("Enter exercise ID: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid selection.");
                Console.ReadKey();
                return;
            }

            // Get the selected exercise by ID
            var selectedEx = loggedInUser.Exercises.FirstOrDefault(e => e.ID == choice);
            if (selectedEx == null)
            {
                Console.WriteLine("Exercise not found!");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Editing '{selectedEx.Name}'\n");

            // Ask for new values (optional)
            Console.Write($"New name (leave blank to keep '{selectedEx.Name}'): ");
            string newName = Console.ReadLine();

            Console.Write($"New duration (current {selectedEx.DurationMinutes}): ");
            string newDurationInput = Console.ReadLine();

            Console.Write($"New description (leave empty to keep current): ");
            string newDescription = Console.ReadLine();

            // MUSCLE GROUP
            Console.WriteLine("New muscle group (leave blank to keep current)");
            Console.WriteLine("1. Legs2. Chest3. Back4. Arms5. Core");
            string newMG = Console.ReadLine();

            // DIFFICULTY LEVEL
            Console.WriteLine("\nNew difficulty (leave blank to keep current)");
            Console.WriteLine("1. Easy2. Medium3. Hard");
            string newDiff = Console.ReadLine();

            // APPLY CHANGES IF USER ENTERED NEW VALUES
            if (!string.IsNullOrWhiteSpace(newName))
                selectedEx.Name = newName;

            if (int.TryParse(newDurationInput, out int newDuration))
                selectedEx.DurationMinutes = newDuration;

            if (!string.IsNullOrWhiteSpace(newDescription))
                selectedEx.Description = newDescription;

            if (!string.IsNullOrWhiteSpace(newMG))
            {
                selectedEx.MuscleGroup = newMG switch
                {
                    "1" => "Legs",
                    "2" => "Chest",
                    "3" => "Back",
                    "4" => "Arms",
                    "5" => "Core",
                    _ => selectedEx.MuscleGroup // Keep original if invalid
                };
            }

            if (!string.IsNullOrWhiteSpace(newDiff))
            {
                selectedEx.Difficulty = newDiff switch
                {
                    "1" => "Easy",
                    "2" => "Medium",
                    "3" => "Hard",
                    _ => selectedEx.Difficulty // Keep original if invalid
                };
            }

            // Save updated data
            userDataService.SaveUser(loggedInUser);

            Console.WriteLine("Exercise updated!");
            Console.ReadKey();
        }

        public void DeleteExercise()
        {
            Console.Clear();

            // Check if the user has ANY exercises
            if (loggedInUser.Exercises.Count == 0)
            {
                Console.WriteLine("You have no exercises to delete.");
                Console.ReadKey();
                return;
            }

            // Show exercises so user can choose what to delete
            Console.WriteLine("Select an exercise to delete:\n");
            foreach (var ex in loggedInUser.Exercises)
            {
                Console.WriteLine($"ID: {ex.ID} | {ex.Name}");
            }

            Console.Write("\nEnter exercise ID: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid selection.");
                Console.ReadKey();
                return;
            }

            var selectedExercise = loggedInUser.Exercises.FirstOrDefault(e => e.ID == choice);
            if (selectedExercise == null)
            {
                Console.WriteLine("Exercise ID not found!");
                Console.ReadKey();
                return;
            }

            // Confirm deletion
            Console.Write($"\nAre you sure you want to delete '{selectedExercise.Name}'? (y/n): ");
            string confirm = Console.ReadLine().ToLower();

            if (confirm != "y")
            {
                Console.WriteLine("Deletion canceled.");
                Console.ReadKey();
                return;
            }

            // Remove the exercise from the list
            loggedInUser.Exercises.Remove(selectedExercise);

            // Save changes to JSON
            userDataService.SaveUser(loggedInUser);

            Console.WriteLine("Exercise deleted successfully!");
            Console.ReadKey();
        }
    }
}

