using System.IO;
using System.Reflection;
using System.Threading;
using System;
using checkvalidation;
namespace MyProgram
{
    public class NumberValidator
    {
        public static int number(string num)
        {
            int a;
            while (!int.TryParse(num, out a))
            {
                Console.WriteLine("Please Only Enter Number");
                num = Console.ReadLine();
            }
            return a;
        }
    }
    public partial class program1
    {
        static string directoryPath;
        static string userFilePath;
        static string username;
        static string password;
        public static bool isLoggedIn = false;
        public static int ctr = 0;
        static program1()
        {
            string pathTo = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo Dinfo = new DirectoryInfo(pathTo).Parent.Parent.Parent;
            directoryPath = Path.Combine(Dinfo.FullName, "Output");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            userFilePath = Path.Combine(directoryPath, "user.txt");
            LoadLastUser();
        }
        static void LoadLastUser()
        {
            string[] lines = File.ReadAllLines(userFilePath);
            if (lines.Length > 0)
            {
                string lastUser = lines[lines.Length - 1];
                string[] parts = lastUser.Split(',');
                if (parts.Length == 2)
                {
                    username = parts[0];
                    password = parts[1];
                    ctr = int.Parse(username.Substring(5, username.IndexOf('@') - 5));
                }
            }
        }
        public static void display()
        {
            while (!isLoggedIn)
            {
                Console.WriteLine("Press 1: Login");
                Console.WriteLine("Press 2: Create New User");
                Console.WriteLine("Enter your choice: ");
                int choice = NumberValidator.number(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Login();
                        LogoutAndExit();
                        break;
                    case 2:
                        CreateNewUser();
                        Thread loginThread = new Thread(display);
                        loginThread.Start();
                        loginThread.Join();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }
        public static void LogoutAndExit()
        {
            while (!isLoggedIn)
            {
               
                Console.WriteLine("Press 1: Logout");
                Console.WriteLine("Press 2: Exit");
                Console.WriteLine("Enter your choice: ");
                int choice = NumberValidator.number(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Logout();
                        break;
                    case 2:
                        isLoggedIn = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }
        public static void Login()
        {
            Thread.Sleep(1000);
            Console.WriteLine($"User '{username}' logged in.");
            Birthday();
        }
        public static void Logout()
        {
            isLoggedIn = false;
            Console.WriteLine($"User '{username}' logged out.");
            Thread loginThread = new Thread(display);
            loginThread.Start();
            loginThread.Join();
        }
        public static void CreateNewUser()
        {
            Thread.Sleep(1000);
            ctr++;
            username = $"manya{ctr}@gmail.com";
            Random rnd = new Random();
            password = rnd.Next(10000, 99999).ToString();

            using (StreamWriter writer = File.AppendText(userFilePath))
            {
                writer.WriteLine($"{username},{password}");
            }
            Console.WriteLine($"User '{username}' created.");
        }
    }
    class Program
    {
        static void Main()
        {
            program1.display();
        }
    }
}