using System.Collections.Generic;
using System.Globalization;
using System;
using System.IO;
namespace MyProgram
{
    public partial class program1
    {
        public static void Birthday()
        {
            Console.Write("Enter the filename containing employee names and birthdates: ");
            string filename = Console.ReadLine();
            List<Employee> employees = ReadEmployeesFromFile(filename);
            if (employees != null)
            {
                List<Employee> upcomingBirthdays = FindUpcomingBirthdays(employees, 10);

                if (upcomingBirthdays.Count > 0)
                {
                    Console.WriteLine("\nUpcoming birthdays within the next 10 days:\n");
                    foreach (var employee in upcomingBirthdays)
                    {
                        Console.WriteLine($"{employee.Name}: {employee.DateOfBirth.ToString("dd-MM-yyyy")}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo upcoming birthdays within the next 10 days.");
                }
            }
            else
            {
                Console.WriteLine("\nError reading the file. Please check the filename and try again.");
            }
        }
        static List<Employee> ReadEmployeesFromFile(string filename)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string[] lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 2)
                    {
                        string name = parts[0];
                        DateTime dob;
                        if (DateTime.TryParseExact(parts[1], "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                        {
                            employees.Add(new Employee { Name = name, DateOfBirth = dob });
                        }
                    }
                }
                return employees;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return null;
            }
        }
        static List<Employee> FindUpcomingBirthdays(List<Employee> employees, int days)
        {
            DateTime today = DateTime.Today;
            DateTime upcomingDateLimit = today.AddDays(days);
            List<Employee> upcomingBirthdays = new List<Employee>();
            foreach (var employee in employees)
            {
                DateTime nextBirthday = new DateTime(today.Year, employee.DateOfBirth.Month, employee.DateOfBirth.Day);
                if (nextBirthday < today)
                {
                    nextBirthday = nextBirthday.AddYears(1);
                }
                if (nextBirthday <= upcomingDateLimit)
                {
                    upcomingBirthdays.Add(employee);
                }
            }
            return upcomingBirthdays;
        }
    }
    class Employee
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}