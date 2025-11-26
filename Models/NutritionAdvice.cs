using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models
{
    public class NutritionAdvice
    {
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public string Tip { get; set; } // Additional tip or advice
        public override string ToString()
        {
            return $"Calories: {Calories:F0} kcal\nProtein: {Protein:F0}g | Carbs: {Carbs:F0}g | Fat: {Fat:F0}g\n{Tip}";
        }
    }
}