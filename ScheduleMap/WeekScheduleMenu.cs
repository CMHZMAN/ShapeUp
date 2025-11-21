using ShapeUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.ScheduleMap
{
    public class WeekScheduleMenu
    {
        private readonly ScheduleService scheduleService;

        // Constructor receives the logged-in user and initializes the ScheduleService
        public WeekScheduleMenu(User user)
        {
            scheduleService = new ScheduleService(user);
        }

        // Main menu loop
        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== WEEK SCHEDULE MENU ===");
                Console.WriteLine("1. Create Week");
                Console.WriteLine("2. Add Exercise to Week");
                Console.WriteLine("3. Remove Exercise from Week");
                Console.WriteLine("4. View Week");
                Console.WriteLine("5. View All Weeks");
                Console.WriteLine("6. Delete Week");
                Console.WriteLine("0. Back");
                Console.Write("Choose: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        scheduleService.AddWeek();
                        break;

                    case "2":
                        scheduleService.AddExerciseToWeek();
                        break;

                    case "3":
                        scheduleService.RemoveExerciseFromWeek();
                        break;

                    case "4":
                        scheduleService.ViewWeek();
                        break;

                    case "5":
                        scheduleService.ViewAllWeeks();
                        break;

                    case "6":
                        scheduleService.DeleteWeek();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid choice!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
