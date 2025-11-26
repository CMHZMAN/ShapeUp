using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models
{
    public class WorkoutSession
    {
        public DateTime Date { get; set; }
        public string ExerciseName { get; set; }
        public List<ExerciseSet> Sets { get; set; }

        public WorkoutSession()
        {
            Sets = new List<ExerciseSet>();
            Date = DateTime.Now;
        }
    }
}
