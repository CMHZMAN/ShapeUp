using ShapeUp.Models;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ShapeUp.Services
{
    public class Nutrition_Calculator

    {
        public static class NutritionCalculator     // Nutrition Calculator Service
        {
            public static NutritionAdvice GetNutritionAdvice(double weight, double height, int age, string gender, string activityLevel, string goal)
            {
                double bmr = CalculateBmr(weight, height, age, gender);
                double activityFactor = GetActivityFactor(activityLevel);
                double tdee = bmr * activityFactor;     // Total Daily Energy Expenditure
                double calories = AdjustForGoal(tdee, goal);
                var (protein, carbs, fat) = CalculateMacros(calories);
                string tip = GetProteinTip(weight, goal);

                return new NutritionAdvice  // Return Nutrition Advice Object
                {
                    Calories = calories,
                    Protein = protein,
                    Carbs = carbs,
                    Fat = fat,
                    Tip = tip
                };
            }

            public static double CalculateBmr(double weight, double height, int age, string gender)
            {
                if (gender?.ToLower() == "male")
                    return 88.36 + (13.4 * weight) + (4.8 * (height * 100)) - (5.7 * age);
                else
                    return 447.6 + (9.2 * weight) + (3.1 * (height * 100)) - (4.3 * age);
            }

            public static double GetActivityFactor(string level)
            {
                return level.ToLower()

                switch
                {
                    "sedentary" => 1.2, 
                    "light" => 1.375,
                    "moderate" => 1.55,
                    "active" => 1.725,
                    "very active" => 1.9,
                    _ => 1.2
                };
            }

            public static double AdjustForGoal(double tdee, string goal)    //tdee: Total Daily Energy Expenditure
            {
                return goal.ToLower()

                switch
                {
                    "lose" => tdee - 400,
                    "gain" => tdee + 400,
                    _ => tdee
                };
            }

            public static (double protein, double carbs, double fat) CalculateMacros(double calories)
            {
                double protein = (calories * 0.3) / 4;
                double carbs = (calories * 0.5) / 4;
                double fat = (calories * 0.2) / 9;
                return (protein, carbs, fat);
            }

            public static string GetProteinTip(double weight, string goal)
            {
                return goal.ToLower()

                switch
                {
                    "gain" => "Aim for around 2.0g of protein per kg of body weight to support muscle growth.",
                    "lose" => "Keep protein high (~1.8g/kg) to maintain muscle while cutting.",
                    _ => "1.6–2.0g of protein per kg is optimal for maintaining health and recovery."
                };
            }
        }
    }
}