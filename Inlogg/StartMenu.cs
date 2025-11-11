using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeUp.Inlogg
{
    public class StartMenu
    {
        private readonly Register register;
        private readonly Login login;

        public StartMenu()
        {
            register = new Register();
            login = new Login();
        }

        public void Menu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("1. Register new user");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        register.CreateUser();
                        Pause();
                        break;

                    case "2":
                        login.SignIn();
                        Pause();
                        break;

                    case "3":
                        Console.WriteLine("Exiting program...");
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        Pause();
                        break;
                }
            }
        }

        private void Pause()
        {
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }
    }


}
