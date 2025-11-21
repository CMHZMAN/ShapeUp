using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models.Exercises
{
    public class Exercise
    {
        public int ID { get; set; }                  // Unique ID for the exercise
        public string Name { get; set; }            // The name of the exercise (e.g. "Running")
        public int DurationMinutes { get; set; }     // How long the exercise lasts (in minutes)
        public string Description { get; set; }     // e.g. "Jog at light pace"
        public string MuscleGroup { get; set; }     // e.g. "Legs", "Chest", "Back"
        public string Difficulty { get; set; }      // e.g. "Easy", "Medium", "Hard"
    }
}
