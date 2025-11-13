using ShapeUp.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string Pending2FACode { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double Age { get; set; }
        public string Gender { get; set; }


        // Store all exercises created by this USER in a list
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();


        //
        public Dictionary<string, string> WeeklySchedule { get; set; } = new Dictionary<string, string>();
    }
}
