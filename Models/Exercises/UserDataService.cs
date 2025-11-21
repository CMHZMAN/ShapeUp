using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    public class UserDataService
    {
        // Path to the JSON file that stores all users
        private const string FilePath = "user.json";

        // LOAD USERS
        public List<User> LoadUsers()
        {
            // If the file does not exist, return an empty list of users
            if (!File.Exists(FilePath))
                return new List<User>();

            try
            {
                // Read all content from the JSON file
                string json = File.ReadAllText(FilePath);

                // Deserialize JSON into a list of User objects
                // If deserialization fails or returns null, return an empty list
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                // If reading or deserialization fails for any reason, return empty list
                return new List<User>();
            }
        }

        // SAVE A SINGLE USER
        public void SaveUser(User updatedUser)
        {
            // Load all users from JSON
            List<User> allUsers = LoadUsers();

            // Check if the user already exists in the list (matching by ID)
            var existing = allUsers.FirstOrDefault(u => u.ID == updatedUser.ID);

            if (existing != null)
            {
                // If the user exists, replace the old version with the updated one
                int index = allUsers.IndexOf(existing);
                allUsers[index] = updatedUser;
            }
            else
            {
                // If the user is new, add them to the list
                allUsers.Add(updatedUser);
            }

            // Serialize the updated list of users back to JSON, with indented formatting
            string json = JsonSerializer.Serialize(allUsers, new JsonSerializerOptions { WriteIndented = true });

            // Write the JSON back to the file
            File.WriteAllText(FilePath, json);
        }
    }


}

