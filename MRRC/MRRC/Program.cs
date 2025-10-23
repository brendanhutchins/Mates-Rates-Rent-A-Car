using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using MRRCManagement;

namespace MRRC
{
    /// <summary>
    ///
    /// Menu driven program that allows the user to navigate between 
    /// customer, fleet, and rental operations. The program can read
    /// CSV files and save instances of CRM and Fleet lists.
    /// 
    /// This class handles any outside input such as external files,
    /// which can be accessed through the Debug information.
    ///
    /// Author Brendan Hutchins May 2020
    ///
    ///
    /// </summary>
    class Program
    {
        static string customers = @"..\..\..\Data\customers.csv";
        static string fleet = @"..\..\..\Data\fleet.csv";
        static string rentals = @"..\..\..\Data\rentals.csv";

        public static void Main(string[] args)
        {
            ValidateInput(args);

            CRM customerList = new CRM(customers);
            Fleet fleetList = new Fleet(fleet, rentals);

            Menu.WelcomeMenu();
            Menu.customerList = customerList;
            Menu.fleetList = fleetList;
            Menu.rentalDict = fleetList.Rentals;
            Menu.MainMenu();
        }

        // Take the supplied arguments from the main function and plug them into
        // the customer, fleet, and/or rental files
        private static void ValidateInput(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.Length == 1)
                {
                    customers = args[0];
                    if (!File.Exists(customers))
                    {
                        Console.WriteLine("No customer file exists at this location!\nPress any key to exit.");
                        Menu.ExitProgram();
                    }
                }
                else if (args.Length == 2)
                {
                    customers = args[0];
                    fleet = args[1];
                    if (!File.Exists(customers) || !File.Exists(fleet))
                    {
                        Console.WriteLine("No customer or fleet file exists at this location!\nPress any key to exit.");
                        Console.ReadLine();
                        Menu.ExitProgram();
                    }
                }
                else if (args.Length == 3)
                {
                    customers = args[0];
                    fleet = args[1];
                    rentals = args[2];
                    if (!File.Exists(customers) || !File.Exists(fleet) || !File.Exists(rentals))
                    {
                        Console.WriteLine("No customer, fleet, or rental file exists at this location!\nPress any key to exit.");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}
