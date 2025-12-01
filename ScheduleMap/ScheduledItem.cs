using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.ScheduleMap
{
    public class ScheduledItem
    {
        public int Id { get; set; }               // Unique ID within the week
        public DayOfWeek Day { get; set; }        // Day of the week
        public TimeSpan TimeOfDay { get; set; }   // Time of day
        public string ItemType { get; set; }      // "Exercise" or "Meal"
        public int ItemId { get; set; }           // Reference to the Exercise or Meal ID
    }
}
