using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    internal class UserDataService
    {
            private const string FilePath = "user.json";

            public List<User> LoadUsers()
            {
                if (!File.Exists(FilePath))
                    return new List<User>();

                try
                {
                    string json = File.ReadAllText(FilePath);
                    return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                }
                catch
                {
                    return new List<User>();
                }
            }

            public void SaveUser(User updatedUser)
            {
                List<User> allUsers = LoadUsers();

                var existing = allUsers.FirstOrDefault(u => u.ID == updatedUser.ID);
                if (existing != null)
                {
                    int index = allUsers.IndexOf(existing);
                    allUsers[index] = updatedUser;
                }
                else
                {
                    allUsers.Add(updatedUser);
                }

                string json = JsonSerializer.Serialize(allUsers, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
        }
    }

