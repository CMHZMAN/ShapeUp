using ShapeUp.Models.Exercises;  // For Exercise class
using ShapeUp.ScheduleMap;       // For Schedule class
using MealModel = ShapeUp.Models.Meal.Meal;
using ShapeUp.Models.Meal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models
{
    public class User
    {
        public int ID { get; set; }                  // User ID
        public string Username { get; set; }         // Username
        public string Password { get; set; }         // Password
        public string Contact { get; set; }          // Contact info (email/phone)
        public string Pending2FACode { get; set; }   // 2FA code (temporary)
        public double Height { get; set; }           // Height in cm
        public double Weight { get; set; }           // Weight in kg
        public double Age { get; set; }                 // Age in years
        public string Gender { get; set; }           // Gender

        // List of exercises created by the user
        public List<Exercise> Exercises { get; set; } = new();

        // List of meals created by the user
        public List<MealModel> Meals { get; set; } = new();

        // Scheduled weeks with exercises and meals
        public List<Schedule> WeeklyPlans { get; set; } = new();
    }
}
