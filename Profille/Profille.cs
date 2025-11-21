using ShapeUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ShapeUp.Profille
{
    public class ProfileManager
    {
        private const string FilePath = "user.json";

        public void UpdateProfile(User LoggedInUser)
        {
            Console.Write("Height (cm)");
            if (double.TryParse(Console.ReadLine(),out double height))
                LoggedInUser.Height = height;
            Console.Write("Weight (kg)");
            if (double.TryParse(Console.ReadLine(), out double weight))
                LoggedInUser.Weight = weight;
            Console.Write("Gender (Male/Female/Other)");
            LoggedInUser.Gender = Console.ReadLine();

            Console.WriteLine("Age");
            if (int.TryParse(Console.ReadLine(), out int age))
                LoggedInUser.Age = age;


            List<User> users = LoadUSers();

            //Find the correct user id

            var userUpdate = users.FirstOrDefault(u => u.ID == LoggedInUser.ID);
            if (userUpdate != null)
            {
                userUpdate.Height = LoggedInUser.Height;
                userUpdate.Weight = LoggedInUser.Weight;
                userUpdate.Gender = LoggedInUser.Gender;
                userUpdate.Age = LoggedInUser.Age;
                

                SaveUser(users);
                Console.WriteLine("Profile updated successfully.");
            }
            else 
            { 
                Console.WriteLine("User not found.");
            }
        }

        private List<User> LoadUSers()
            {
                if (!File.Exists(FilePath))
                    return new List<User>();

                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                
                
            }
            private void SaveUser(List<User> users)
            {
                    string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                     File.WriteAllText(FilePath, json);
            }






        }       
  }

