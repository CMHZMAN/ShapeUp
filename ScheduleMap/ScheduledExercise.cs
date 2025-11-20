using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.ScheduleMap
{
    public class ScheduledExercise
    {
        public int Id { get; set; }           // Unique ID for this scheduled exercise
        public int ExerciseId { get; set; }   // Reference to the Exercise ID
        public DateTime StartTime { get; set; } // When the exercise starts
    }
}
