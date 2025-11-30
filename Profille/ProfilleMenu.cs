using ShapeUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShapeUp.Profille
{
    public class ProfilleMenu
    {
        private readonly User loggedInUser;
        private readonly ProfileManager profileManager;

        public ProfilleMenu(User user)
        {
            loggedInUser = user;
             profileManager= new ProfileManager();
        }
        public void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Meal Menu");
                Console.WriteLine("1. View Profile");
                Console.WriteLine("2. Update Profile");
                Console.WriteLine("0. Back to User Menu");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowProffile();
                        Pause();
                        break;
                    case "2":
                        profileManager.UpdateProfile(loggedInUser);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadKey();
                        break;
                }

            }
        }
        private void ShowProffile()
        {
            Console.WriteLine("User Profile");
            Console.WriteLine($"Username: {loggedInUser.Username}");
            Console.WriteLine($"Height: {loggedInUser.Height} cm");
            Console.WriteLine($"Weight: {loggedInUser.Weight} kg");
            Console.WriteLine($"age: {loggedInUser.Age}");
        }

        private void Pause()
        {
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

    }
}
