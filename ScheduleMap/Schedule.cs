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
        public int Id { get; set; }                      // Unique week ID
        public int WeekNumber { get; set; }             // Week number 1–52
        public List<ScheduledItem> Items { get; set; }  // Scheduled exercises or meals
    }
}
