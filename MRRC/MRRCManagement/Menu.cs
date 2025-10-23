using MRRC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    /// <summary>
    ///
    /// Menu driven program that allows the user to navigate between 
    /// customer, fleet, and rental operations. The program can read
    /// CSV files and save instances of CRM and Fleet lists.
    /// 
    /// This class handles the Menu navigation. Entering ESCAPE allows
    /// the user to exit the program while in a menu operation. 
    /// Entering BACKSPACE allows the user to go back to main menu.
    ///
    /// Author Brendan Hutchins May 2020
    ///
    ///
    /// </summary>
    public class Menu
    {
        public static CRM customerList;
        public static Fleet fleetList;
        public static Dictionary<string,int> rentalDict;

        // Starting menu for when the user boots up the program
        public static void WelcomeMenu()
        {
            string menu = "\n### Mates-Rates Rent-a-Car Operation Menu ###"
                        + "\n\nYou may press the ESC key at any time to exit. "
                        + "Press the BACKSPACE key to return to the previous menu.";

            Console.Write(menu);
        }

        // Main navigation menu to change between customer, fleet, and rental submenus
        public static void MainMenu()
        {
            int optionAmt = 3;
            int menuOption;

            string menu = "\n\nPlease enter a number from the options below:"
                        + "\n\n1) Customer Management\n\r2) Fleet Management\n\r3) Rental Management\n\n>";

            Console.Write(menu);

            menuOption = ReadInput.ReadOption(optionAmt, menu);

            // Use the result returned from ReadOption to select the submenu
            switch (menuOption)
            {
                case 1:
                    CustomerMenu();
                    break;
                case 2:
                    FleetMenu();
                    break;
                case 3:
                    RentalMenu();
                    break;
            }
        }

        // Menu handling CRM operations
        public static void CustomerMenu()
        {
            int optionAmt = 4;
            int menuOption;
            bool didWork;

            string menu = "\n\nPlease enter a number from the options below:"
                       + "\n\n1) Display Customers\n\r2) New Customer"
                       + "\n\r3) Modify Customer\n\r4) Delete Customer\n\n>";

            Console.Write(menu);

            menuOption = ReadInput.ReadOption(optionAmt, menu);

            MenuHelpers.customerList = customerList;
            MenuHelpers.fleetList = fleetList;

            switch (menuOption)
            {
                case 1:
                    // Display Customers
                    List<Customer> displayList = customerList.GetCustomers();
                    Table table = new Table();
                    Console.Write("\n\n");
                    string[] headers = { "ID", "Title", "FirstName", "LastName", "Gender", "DOB" };
                    string[] rows = table.GetRows(headers, displayList);
                    if (rows.Length == 0)
                    {
                        Console.WriteLine("\nNo Customers in this list yet");
                    }
                    else
                    {
                        table.PrintHeader(headers, displayList);
                        foreach (string row in rows)
                        {
                            Console.WriteLine(row);
                        }
                    }
                    break;
                case 2:
                    // New Customer
                    do
                    {
                        Customer newCustomer = MenuHelpers.NewCustomer();
                        didWork = customerList.AddCustomer(newCustomer);
                        if (!didWork)
                        {
                            Console.WriteLine("Error adding customer, try again.");
                        }
                    } while (!didWork) ;

                    break;
                case 3:
                    // Modify Customer
                    MenuHelpers.ModifyCustomer();
                    break;
                case 4:
                    // Delete Customer
                    MenuHelpers.DeleteCustomer();
                    break;
            }

            CustomerMenu();
        } // end of CustomerMenu()

        // Menu handling Fleet operations
        public static void FleetMenu()
        {
            int optionAmt = 4;
            int menuOption;
            bool didWork;

            string menu = "\n\nPlease enter a number from the options below:"
                       + "\n\n1) Display Fleet\n\r2) New Vehicle"
                       + "\n\r3) Modify Vehicle\n\r4) Delete Vehicle\n\n>";

            Console.Write(menu);

            menuOption = ReadInput.ReadOption(optionAmt, menu);

            MenuHelpers.fleetList = fleetList;

            switch (menuOption)
            {
                case 1:
                    // Display Fleet
                    List<Vehicle> displayList = fleetList.GetFleet();
                    MenuHelpers.DisplayVehicle(displayList);
                    break;
                case 2:
                    //    New Vehicle
                    do 
                    {
                        Vehicle newVehicle = MenuHelpers.NewVehicle();
                        didWork = fleetList.AddVehicle(newVehicle);
                        if (!didWork)
                        {
                            Console.WriteLine("Error adding customer, try again.");
                        }
                    } while (!didWork) ;
                    break;
                case 3:
                    //    Modify Vehicle
                    MenuHelpers.ModifyVehicle();
                    break;
                case 4:
                    // Delete Vehicle
                    MenuHelpers.DeleteVehicle();
                    break;
            }

            FleetMenu();
        }

        // Menu handling Rental operations
        public static void RentalMenu()
        {
            int optionAmt = 4;
            int menuOption;

            string menu = "\n\nPlease enter a number from the options below:"
                       + "\n\n1) Display Rentals\n\r2) Search Vehicles"
                       + "\n\r3) Rent Vehicle\n\r4) Return Vehicle\n\n>";

            Console.Write(menu);

            menuOption = ReadInput.ReadOption(optionAmt, menu);

            // Initialize lists in MenuHelpers
            MenuHelpers.customerList = customerList;
            MenuHelpers.fleetList = fleetList;
            MenuHelpers.rentalDict = rentalDict;

            string returnMenu = "rental";

            switch (menuOption)
            {
                case 1:
                    // Display Rentals
                    List<KeyValuePair<string,int>> rentalTableList = rentalDict.ToList();

                    Table table = new Table();
                    Console.Write("\n\n");
                    string[] headers = { "Registration", "ID" };
                    string[] rows = table.GetRows(headers, rentalTableList);
                    // If rental list is empty
                    if (rows.Length == 0)
                    {
                        Console.WriteLine("\nNo Rentals in this list yet");
                    }
                    else
                    {
                        table.PrintHeader(headers, rentalTableList);
                        foreach (string row in rows)
                        {
                            Console.WriteLine(row);
                        }
                    }
                    break;
                case 2:
                    //    Search Vehicles
                    Console.Write("\n\nEnter search query here, enter -1 to go back: ");
                    string userInput = Console.ReadLine();
                    try
                    {
                        List<Token> infixTokens = Search.ParseText(userInput);

                        // Output full list of unrented vehicles, does not show if ParseText
                        // fails (user types -1 to go back to menu)
                        Console.Write("\n\r------------Searching------------");
                        List<Vehicle> initialList = fleetList.GetFleet(false);
                        MenuHelpers.DisplayVehicle(initialList);

                        Console.WriteLine("\nInfix Expression...");
                        foreach (Token token in infixTokens)
                            Console.Write($"{token} ");
                        Console.WriteLine("\n");

                        // Postfix Notation
                        List<Token> postfixTokens = Search.ShuntingYard(infixTokens);
                        Console.WriteLine("Postfix Expression...");
                        foreach (Token token in postfixTokens)
                            Console.Write($"{token} ");
                        Console.WriteLine("\n");

                        // Output result
                        Console.Write("Search result:");
                        List<Vehicle> searchedVehicles = Search.SearchVehicles(postfixTokens, fleetList);

                        if (searchedVehicles.Count == 0)
                        {
                            Console.WriteLine("\nNo unrented vehicles that match query were found!");
                        }
                        else
                        {
                            MenuHelpers.DisplayVehicle(searchedVehicles);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                case 3:
                    //    Rent Vehicle
                    MenuHelpers.RentVehicle();
                    break;
                case 4:
                    // Return Vehicle
                    bool didWork;
                    string registration;
                    
                    Console.WriteLine("\n\nPlease enter a vehicle registration to return."
                            + "\nIf You wish to go back to the previous menu, enter -1 into the field.\n");
                    // Get registration
                    do
                    {
                        do
                        {
                            // Call Read Registration helper
                            registration = ReadInput.ReadRegistration(returnMenu);

                            // Check if the registration given is an existing vehicle
                            List<Vehicle> checkList = fleetList.GetFleet();
                            didWork = checkList.Exists(item => item.registration == registration);
                            if (!didWork)
                            {
                                Console.WriteLine("Registration not found, please try again!");
                            }
                        } while (!didWork);

                        try
                        {
                            int customerID = fleetList.RentedBy(registration);
                            if (customerID == -1)
                            {
                                throw new CurrentlyRentedException("Vehicle provided not currently rented, please try again!");
                            }
                            Console.Write($"Are you sure you wish to return vehicle {registration} from customer {customerID}? y/n: ");
                            string input = Console.ReadLine();
                            if (input == "y" || input == "Y")
                            {
                                // return vehicle
                                fleetList.ReturnVehicle(registration);
                                Console.WriteLine($"Vehicle sucessfully returned from customer {customerID}"
                                                    + $"\r\nPress enter to continue.");
                                Console.ReadLine();
                                didWork = true;
                            }
                            else
                            {
                                Console.WriteLine($"Vehicle {registration} not returned, press enter to continue to main menu");
                                Console.ReadLine();
                                RentalMenu();
                                break;
                            }
                        }

                        catch(CurrentlyRentedException e)
                        {
                            Console.WriteLine(e.Message);
                            didWork = false;
                        }
                    } while (!didWork);
                    break;
            }

            RentalMenu();
        }

        // Handle exiting the program gracefully and save files before exiting
        public static void ExitProgram()
        {
            ConsoleKeyInfo userChoice;

            Console.Write("\n\nPress enter to save and exit. Press BACKSPACE to go back to main menu.\n");
            userChoice = Console.ReadKey();

            // If user decides to go back return to main menu
            if (userChoice.Key == ConsoleKey.Backspace)
            {
                MainMenu();
            }

            customerList.SaveToFile();
            fleetList.SaveVehiclesToFile();
            fleetList.SaveRentalsToFile();

            System.Environment.Exit(0);
        }
    }
}

