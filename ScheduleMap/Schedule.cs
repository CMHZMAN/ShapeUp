using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.ScheduleMap
{
    // Represents a scheduled exercise in a day
    public class Schedule
    {
        public int Id { get; set; }                 // Unique ID for the week
        public int WeekNumber { get; set; }         // Week number for ordering
        public List<ScheduledExercise> Exercises { get; set; } = new List<ScheduledExercise>();
    }
}
