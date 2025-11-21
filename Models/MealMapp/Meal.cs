using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Meal
{
    public class Meal
    {
        public int Id { get; set; }
        public string MealName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public int TotalCalories { get; set; }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public int Calories { get; set; }
    }
}