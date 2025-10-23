using MRRC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Menu driven program that allows the user to navigate between
    // customer, fleet, and rental operations. The program can read
    // CSV files and save instances of CRM and Fleet lists.
    //
    // This class was created to help declutter the menu class and handle the backend work of adding,
    // deleting, and modifying vehicles and customers. Also handles rent vehicle. Commonly references 
    // ReadInput class for userInput and parameter checking.
    //
    // Author Brendan Hutchins May 2020
    public class MenuHelpers
    {
        public static CRM customerList;
        public static Fleet fleetList;
        public static Dictionary<string, int> rentalDict;
        static bool isValid;
        static string userInput;
        
        // Helper method for AddCustomer
        public static Customer NewCustomer()
        {
            string prompt;

            Gender gender;
            DateTime DOB;
            bool didWork;
            string menu = "customer";

            // Header message
            Console.WriteLine("\nPlease fill the following fields (fields marked with * are required)"
                            + "\nIf You wish to go back to the previous menu, enter -1 into any field.\n");

            // ID
            prompt = "ID*: ";
            int ID;

            do
            {
                ID = ReadInput.ReadInt(prompt, menu);
                // Check if ID is taken
                List<Customer> customersList = customerList.GetCustomers();
                if (customersList.Any(n => n.customerID == ID))
                {
                    didWork = false;
                    Console.WriteLine("ID has been taken! Please try again.");
                }
                else { didWork = true; }
            } while (!didWork);

            // Title
            prompt = "Title*: ";
            string title = ReadInput.ReadString(prompt, menu);

            // First Name
            prompt = "FirstName*: ";
            string firstName = ReadInput.ReadString(prompt, menu);

            // Last Name
            prompt = "LastName*: ";
            string lastName = ReadInput.ReadString(prompt, menu);

            // Gender
            prompt = "Gender*: ";
            do
            {
                string input = ReadInput.ToCapitals(prompt, menu);

                // Try converting input to an enum
                isValid = Enum.TryParse(input, out gender);
                if (!isValid)
                {
                    if (input == "-1")
                    {
                        Menu.CustomerMenu();
                    }
                    else
                    {
                        isValid = false;
                        Console.Write("Incorrect input detected, please try again.\n");
                    }
                }
            } while (!isValid);

            // Date of Birth
            do
            {
                Console.Write("DOB*: ");
                userInput = Console.ReadLine();

                isValid = DateTime.TryParse(userInput, new CultureInfo("fr-FR"), DateTimeStyles.None, out DOB);
                if (!isValid)
                {
                    if (userInput == "-1")
                    {
                        Menu.CustomerMenu();
                    }
                    else
                    {
                        isValid = false;
                        Console.Write("Incorrect format of date detected, please try again.\n");
                    }
                }
            } while (!isValid);

            Customer newCustomer = new Customer(ID, title, firstName, lastName, gender, DOB);

            return newCustomer;
        } // End of NewCustomer

        // Helper method for Modify Customer
        public static void ModifyCustomer()
        {
            bool didWork;
            string menu = "customer";
            string prompt;
            int choice;

            int ID;
            Gender gender;
            DateTime DOB;
            Customer modifyCustomer;

            // Header instructions
            Console.WriteLine("\n\nPlease enter a customer ID to modify."
                            + "\nIf You wish to go back to the previous menu, enter -1 into any field.\n");
            do
            {
                // Call ReadID helper
                prompt = "ID*: ";
                ID = ReadInput.ReadInt(prompt, menu);

                // Check if the ID given is an existing customer
                List<Customer> modifyList = customerList.GetCustomers();
                didWork = modifyList.Exists(item => item.customerID == ID);
                if (!didWork)
                {
                    Console.WriteLine("ID not found, please try again!");
                }
            } while (!didWork);

            bool isFinished = false;
            // Start of isFinished loop, loops as long as user wishes to input more values to edit the customer
            do
            {
                // Output field choices for users to edit
                do
                {
                    modifyCustomer = customerList.GetCustomer(ID);
                    // Output the selected customer information
                    Console.WriteLine("\n" + modifyCustomer);
                    Console.Write("1. ID\t\t\t\t4. LastName\r\n2. Title\t\t\t5. Gender\r\n3. FirstName\t\t\t6. DOB\r\n>");

                    string userInput = Console.ReadLine();
                    didWork = Int32.TryParse(userInput, out choice);
                    if (!didWork || choice > 6 || choice < -1 || choice == 0)
                    {
                        Console.WriteLine("Please enter a valid number");
                        didWork = false;
                    }
                } while (!didWork);

                // Switch based on user decision above from 1-6, -1 to go back
                switch (choice)
                {
                    case -1:
                        Menu.CustomerMenu();
                        break;

                    case 1:
                        // ID
                        prompt = "ID*: ";
                        do
                        {
                            ID = ReadInput.ReadInt(prompt, menu);
                            // Check if ID is taken
                            List<Customer> customersList = customerList.GetCustomers();
                            if (customersList.Any(n => n.customerID == ID))
                            {
                                if (modifyCustomer.customerID == ID)
                                {
                                    Console.WriteLine("ID supplied is already the customer's ID!");
                                }
                                else
                                {
                                    Console.WriteLine("ID has been taken! Please try again.");
                                }
                                didWork = false;
                            }
                            else { didWork = true; }
                        } while (!didWork);
                        modifyCustomer.customerID = ID;
                        break;

                    case 2:
                        // Title
                        prompt = "Title*: ";
                        string title = ReadInput.ReadString(prompt, menu);
                        modifyCustomer.title = title;
                        break;

                    case 3:
                        // First Name
                        prompt = "FirstName*: ";
                        string firstName = ReadInput.ReadString(prompt, menu);
                        modifyCustomer.firstName = firstName;
                        break;

                    case 4:
                        // Last Name
                        prompt = "LastName*: ";
                        string lastName = ReadInput.ReadString(prompt, menu);
                        modifyCustomer.lastName = lastName;
                        break;

                    case 5:
                        // Gender
                        prompt = "Gender*: ";
                        do
                        {
                            string input = ReadInput.ToCapitals(prompt, menu);

                            // Try converting input to an enum
                            isValid = Enum.TryParse(input, out gender);
                            if (!isValid)
                            {
                                if (input == "-1")
                                {
                                    Menu.CustomerMenu();
                                }
                                else
                                {
                                    isValid = false;
                                    Console.Write("Incorrect input detected, please try again.\n");
                                }
                            }
                        } while (!isValid);
                        modifyCustomer.gender = gender;
                        break;

                    case 6:
                        // DateTime
                        do
                        {
                            Console.Write("DOB*: ");
                            userInput = Console.ReadLine();

                            isValid = DateTime.TryParse(userInput, new CultureInfo("fr-FR"), DateTimeStyles.None, out DOB);
                            if (!isValid)
                            {
                                if (userInput == "-1")
                                {
                                    Menu.CustomerMenu();
                                }
                                else
                                {
                                    isValid = false;
                                    Console.Write("Incorrect format of date detected, please try again.\n");
                                }
                            }
                        } while (!isValid);
                        modifyCustomer.DOB = DOB;
                        break;
                } // End of switch

                // Handle operation to redo the editing process if user wants to edit another option
                do
                {
                    Console.Write("Would you like to edit another option from the vehicle? (y/n): ");
                    userInput = Console.ReadLine();
                    if (userInput == "Y" || userInput == "y")
                    {
                        isFinished = false;
                        didWork = true;
                    }
                    else if (userInput == "n" || userInput == "N")
                    {
                        isFinished = true;
                        didWork = true;
                    }
                    else
                    {
                        Console.WriteLine("Input not recognized, please answer with 'y' or 'n'.)");
                        didWork = false;
                    }
                } while (!didWork);

            } while (!isFinished);

            // Update customer list
            Menu.customerList = customerList;

        } // End of ModifyCustomer

        // Helper method for Deleting Customer
        public static void DeleteCustomer()
        {
            bool didWork;
            string menu = "customer";
            // Header instructions
            Console.WriteLine("\n\nPlease enter a customer ID to delete."
                            + "\nIf You wish to go back to the previous menu, enter -1 into the field.\n");
            do
            {
                // Call ReadID helper
                string prompt = "ID*: ";
                int ID = ReadInput.ReadInt(prompt, menu);

                // Check if the ID given is an existing customer
                List<Customer> deleteList = customerList.GetCustomers();
                didWork = deleteList.Exists(item => item.customerID == ID);
                if (didWork)
                {
                    Customer deleteCustomer = customerList.GetCustomer(ID);
                    // Output the selected customer information
                    Console.WriteLine("\n" + deleteCustomer);

                    // Check if user wants to delete customer
                    Console.Write("Are you sure you wish to delete this customer? (y/n): ");
                    string userInput = Console.ReadLine();
                    // Remove customer if userInput is y, else do not delete the customer
                    if (userInput == "y" || userInput == "Y")
                    {
                        bool isDeleted = customerList.RemoveCustomer(ID, fleetList);
                        if (isDeleted)
                        {
                            Console.WriteLine("Customer successfully deleted. Press Enter to continue.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Returning to CRM Menu. Press Enter to Continue.");
                            Console.ReadLine();
                            Menu.CustomerMenu();
                        }
                    }
                    else if (userInput == "n" || userInput == "N")
                    {
                        Console.WriteLine("Customer not deleted. Press Enter to continue.");
                        Console.ReadLine();
                        Menu.CustomerMenu();
                    }
                    // For safety of data return to customer menu if other input is detected
                    else
                    {
                        Console.WriteLine("Wrong input expected. Returning to CRM Menu, press Enter to Continue.");
                        Console.ReadLine();
                        Menu.CustomerMenu();
                    }
                }
            } while (!didWork);

            Menu.customerList = customerList;
        } // End of DeleteCustomer

        public static void DisplayVehicle(List<Vehicle> displayList)
        {
            Table table = new Table();
            Console.Write("\n\n");
            string[] headers = { "Registration", "Grade", "Make", "Model", "Year", "NumSeats",
                                         "Transmission", "Fuel", "GPS", "SunRoof", "DailyRate", "Colour" };
            string[] rows = table.GetRows(headers, displayList);

            if (rows.Length == 0)
            {
                Console.WriteLine("\nNo Vehicles in this list yet");
            }
            else
            {
                table.PrintHeader(headers, displayList);
                foreach (string row in rows)
                {
                    Console.WriteLine(row);
                }
            }
        }

        // Helper method for AddVehicle
        public static Vehicle NewVehicle()
        {
            string registration;
            bool didWork;
            bool isDefault = false;
            VehicleGrade grade;
            TransmissionType transmission;
            FuelType fuel;
            Vehicle newVehicle = new Economy();
            string menu = "fleet";

            // Header message
            Console.WriteLine("\nPlease fill the following fields (fields marked with * are required)"
                            + "\nIf You wish to go back to the previous menu, enter -1 into any field.\n");

            // Check if registration has been taken
            do
            {
                registration = ReadInput.ReadRegistration(menu);
                List<Vehicle> vehicleList = fleetList.GetFleet();
                // If vehicle list has any matches with registration
                if(vehicleList.Any(n => n.registration == registration))
                {
                    didWork = false;
                    Console.WriteLine("\nRegistration has been taken! Please try again.");
                }
                else { didWork = true; }

            } while (!didWork);

            // Check if user wants to have default values input for the vehicle
            do
            {
                Console.Write("\nWould you like to input all fields for the vehicle manually? (y/n): ");
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    // Manual input all fields
                    case "y":
                    case "Y":
                        isDefault = false;
                        didWork = true;
                        break;
                    // Default input for all fields after Vehicle Year
                    case "n":
                    case "N":
                        isDefault = true;
                        didWork = true;
                        break;
                    // Return to Fleet Menu
                    case "-1":
                        Menu.FleetMenu();
                        break;
                    default:
                        Console.WriteLine("Wrong input detected, please try again.");
                        didWork = false;
                        break;
                }
            } while (!didWork);
            
            // Get vehicle grade
            string prompt = "Grade*: ";
            do
            {
                string input = ReadInput.ToCapitals(prompt, menu);
                // Try converting input to an enum
                isValid = Enum.TryParse(input, out grade);
                if (!isValid)
                {
                    if (input == "-1")
                    {
                        Menu.FleetMenu();
                    }
                    else
                    {
                        isValid = false;
                        Console.Write("Incorrect input detected, please try again.\n");
                    }
                }
            } while (!isValid);

            // Case sensitve get Make
            prompt = "Make (Case Sensitive)*: ";
            string make = ReadInput.ReadString(prompt, menu);

            // Case sensitve get Model
            prompt = "Model (Case Sensitive)*: ";
            string model = ReadInput.ReadString(prompt, menu);

            // Get Year
            prompt = "Year*: ";
            int year = ReadInput.ReadYear();

            // If the vehicle is going to be created with default parameters, skip other parts
            if (isDefault)
            {
                switch (grade)
                {
                    case VehicleGrade.Economy:
                        newVehicle = new Economy(registration, grade, make, model, year);
                        break;
                    case VehicleGrade.Family:
                        newVehicle = new Family(registration, grade, make, model, year);
                        break;
                    case VehicleGrade.Luxury:
                        newVehicle = new Luxury(registration, grade, make, model, year);
                        break;
                    case VehicleGrade.Commercial:
                        newVehicle = new Commercial(registration, grade, make, model, year);
                        break;
                }
            }
            // If Vehicle is going to be manually entered for all fields
            else
            {
                prompt = "NumSeats*: ";
                int numSeats = ReadInput.ReadInt(prompt, menu);

                // Transmission Type
                prompt = "TransmissionType*: ";
                do
                {
                    string input = ReadInput.ToCapitals(prompt, menu);

                    // Try converting input to an enum
                    isValid = Enum.TryParse(input, out transmission);
                    if (!isValid)
                    {
                        if (input == "-1")
                        {
                            Menu.FleetMenu();
                        }
                        else
                        {
                            isValid = false;
                            Console.Write("Incorrect input detected, please try again.\n");
                        }
                    }
                } while (!isValid);

                // Fuel Type
                prompt = "FuelType*: ";
                do
                {
                    string input = ReadInput.ToCapitals(prompt, menu);

                    // Try converting input to an enum
                    isValid = Enum.TryParse(input, out fuel);
                    if (!isValid)
                    {
                        if (input == "-1")
                        {
                            Menu.FleetMenu();
                        }
                        else
                        {
                            isValid = false;
                            Console.Write("Incorrect input detected, please try again.\n");
                        }
                    }
                } while (!isValid);

                // Has GPS?
                prompt = "Has GPS?(y/n)*: ";
                bool GPS = ReadInput.ReadBool(prompt);

                // Has SunRoof?
                prompt = "Has SunRoof?(y/n)*: ";
                bool sunRoof = ReadInput.ReadBool(prompt);

                // Daily Rate
                double dailyRate;
                prompt = "DailyRate*: ";
                do
                {
                    Console.Write(prompt);
                    userInput = Console.ReadLine();
                    isValid = Double.TryParse(userInput, out dailyRate);
                    if (!isValid || dailyRate < 0)
                    {
                        if (dailyRate != -1)
                        {
                            isValid = false;
                            Console.Write("Incorrect input detected, please try again.\n");
                        }
                        else
                        {
                            Menu.FleetMenu();
                        }
                    }
                } while (!isValid);

                // Non case sensitve get Color
                prompt = "Colour*: ";
                string colour = ReadInput.ToCapitals(prompt, menu);

                switch (grade)
                {
                    case VehicleGrade.Economy:
                        newVehicle = new Economy(registration, grade, make, model, year, numSeats, transmission, fuel, GPS, sunRoof, dailyRate, colour);
                        break;
                    case VehicleGrade.Family:
                        newVehicle = new Family(registration, grade, make, model, year, numSeats, transmission, fuel, GPS, sunRoof, dailyRate, colour);
                        break;
                    case VehicleGrade.Luxury:
                        newVehicle = new Luxury(registration, grade, make, model, year, numSeats, transmission, fuel, GPS, sunRoof, dailyRate, colour);
                        break;
                    case VehicleGrade.Commercial:
                        newVehicle = new Commercial(registration, grade, make, model, year, numSeats, transmission, fuel, GPS, sunRoof, dailyRate, colour);
                        break;
                }
            } // End of All Fields vehicle implementation

            return newVehicle;
        } // End of NewVehicle

        // Helper method for ModifyVehicle
        public static void ModifyVehicle()
        {
            bool isFinished;
            bool didWork;
            int choice;
            // Initializer values for ModifyVehicle to reference throughout the program
            Vehicle modifyVehicle;
            string registration;
            VehicleGrade grade;
            TransmissionType transmission;
            FuelType fuel;
            string menu = "fleet";
            // Header instructions
            Console.WriteLine("\n\nPlease enter a vehicle to modify."
                            + "\nIf You wish to go back to the previous menu, enter -1 into the field.\n");

            // Ask for registration
            do
            {
                // Call Read Registration helper
                registration = ReadInput.ReadRegistration(menu);

                // Check if the registration given is an existing vehicle
                List<Vehicle> modifyList = fleetList.GetFleet();
                didWork = modifyList.Exists(item => item.registration == registration);
                if (!didWork)
                {
                    Console.WriteLine("Registration not found, please try again!");
                }
            } while (!didWork);

            // Keep looping as long as user wants to input more values to edit
            isFinished = false;
            do
            {
                // Get user choice on what field to edit, if -1 return to FleetMenu
                do
                {
                    modifyVehicle = fleetList.GetVehicle(registration);
                    // Output the selected vehicle information
                    Console.WriteLine("\n" + modifyVehicle);
                    Console.WriteLine("\nPlease select the option you wish to from the list below edit:\n");
                    Console.Write("1. Registration\t\t\t7. Transmission\r\n2. Grade\t\t\t8. Fuel\r\n3. Make\t\t\t\t9. GPS\r\n"
                                + "4. Model\t\t\t10. SunRoof\r\n5. Year\t\t\t\t11. DailyRate\r\n6. NumSeats\t\t\t12. Colour\r\n\n>");
                    string userInput = Console.ReadLine();
                    didWork = Int32.TryParse(userInput, out choice);
                    if (!didWork || choice > 12 || choice < -1 || choice == 0)
                    {
                        Console.WriteLine("Please enter a valid number");
                        didWork = false;
                    }
                } while (!didWork);

                // Main editing switch for 12 editable fields of the vehicle class
                switch (choice)
                {
                    case -1:
                        // Return to the FleetMenu
                        Menu.FleetMenu();
                        break;

                    case 1:
                        // Check if registration has been taken
                        do
                        {
                            registration = ReadInput.ReadRegistration(menu);
                            List<Vehicle> vehicleList = fleetList.GetFleet();
                            if (vehicleList.Any(n => n.registration == registration))
                            {
                                if (modifyVehicle.registration == registration)
                                {
                                    didWork = false;
                                    Console.WriteLine("\nThis vehicle already has this registration! Please try again.");
                                }
                                else
                                {
                                    didWork = false;
                                    Console.WriteLine("\nRegistration has been taken! Please try again.");
                                }
                            }
                            else { didWork = true; }
                        } while (!didWork);
                        modifyVehicle.registration = registration;
                        break;

                    case 2:
                        string prompt = "Grade*: ";
                        do
                        {
                            string input = ReadInput.ToCapitals(prompt, menu);
                            // Try converting input to an enum
                            isValid = Enum.TryParse(input, out grade);
                            if (!isValid)
                            {
                                if (input == "-1")
                                {
                                    Menu.FleetMenu();
                                }
                                else
                                {
                                    isValid = false;
                                    Console.Write("Incorrect input detected, please try again.\n");
                                }
                            }
                        } while (!isValid);
                        modifyVehicle.grade = grade;
                        break;

                    case 3:
                        // Non case sensitve get Make
                        prompt = "Make (case sensitive)*: ";
                        string make = ReadInput.ReadString(prompt, menu);
                        modifyVehicle.make = make;
                        break;

                    case 4:
                        // Non case sensitve get Model
                        prompt = "Model (case sensitive)*: ";
                        string model = ReadInput.ReadString(prompt, menu);
                        modifyVehicle.model = model;
                        break;

                    case 5:
                        // Get Year
                        prompt = "Year*: ";
                        int year = ReadInput.ReadYear();
                        modifyVehicle.year = year;
                        break;

                    case 6:
                        prompt = "NumSeats*: ";
                        int numSeats = ReadInput.ReadInt(prompt, menu);
                        modifyVehicle.numSeats = numSeats;
                        break;

                    case 7:
                        // Transmission Type
                        prompt = "TransmissionType*: ";
                        do
                        {
                            string input = ReadInput.ToCapitals(prompt, menu);

                            // Try converting input to an enum
                            isValid = Enum.TryParse(input, out transmission);
                            if (!isValid)
                            {
                                if (input == "-1")
                                {
                                    Menu.FleetMenu();
                                }
                                else
                                {
                                    isValid = false;
                                    Console.Write("Incorrect input detected, please try again.\n");
                                }
                            }
                        } while (!isValid);
                        modifyVehicle.transmission = transmission;
                        break;

                    case 8:
                        // Fuel Type
                        prompt = "FuelType*: ";
                        do
                        {
                            string input = ReadInput.ToCapitals(prompt, menu);

                            // Try converting input to an enum
                            isValid = Enum.TryParse(input, out fuel);
                            if (!isValid)
                            {
                                if (input == "-1")
                                {
                                    Menu.FleetMenu();
                                }
                                else
                                {
                                    isValid = false;
                                    Console.Write("Incorrect input detected, please try again.\n");
                                }
                            }
                        } while (!isValid);
                        modifyVehicle.fuel = fuel;
                        break;

                    case 9:
                        // Has GPS?
                        prompt = "Has GPS?(y/n)*: ";
                        bool GPS = ReadInput.ReadBool(prompt);
                        modifyVehicle.GPS = GPS;
                        break;

                    case 10:
                        // Has SunRoof?
                        prompt = "Has SunRoof?(y/n)*: ";
                        bool sunRoof = ReadInput.ReadBool(prompt);
                        modifyVehicle.sunRoof = sunRoof;
                        break;

                    case 11:
                        // Daily Rate
                        double dailyRate;
                        prompt = "DailyRate*: ";
                        do
                        {
                            Console.Write(prompt);
                            userInput = Console.ReadLine();
                            isValid = Double.TryParse(userInput, out dailyRate);
                            if (!isValid || dailyRate < 0)
                            {
                                if (dailyRate != -1)
                                {
                                    isValid = false;
                                    Console.Write("Incorrect input detected, please try again.\n");
                                }
                                else
                                {
                                    Menu.FleetMenu();
                                }
                            }
                        } while (!isValid);
                        modifyVehicle.dailyRate = dailyRate;
                        break;

                    case 12:
                        // Non case sensitve get Color
                        prompt = "Colour*: ";
                        string colour = ReadInput.ToCapitals(prompt, menu);
                        modifyVehicle.colour = colour;
                        break;
                } // End of switch statement

                // Ask if user wants to input more information to edit, else stop
                do
                {
                    Console.Write("Would you like to edit another option from the vehicle? (y/n): ");
                    userInput = Console.ReadLine();
                    if (userInput == "Y" || userInput == "y")
                    {
                        isFinished = false;
                        didWork = true;
                    }
                    else if (userInput == "n" || userInput == "N")
                    {
                        isFinished = true;
                        didWork = true;
                    }
                    else
                    {
                        Console.WriteLine("Input not recognized, please answer with 'y' or 'n'.)");
                        didWork = false;
                    }
                } while (!didWork);
                
            } while (!isFinished); // End of user input loop

        } // End of ModifyVehicle

        // Helper method for Delete Vehicle
        public static void DeleteVehicle()
        {
            bool didWork;
            string menu = "fleet";
            // Header instructions
            Console.WriteLine("\n\nPlease enter a vehicle registration to delete."
                            + "\nIf You wish to go back to the previous menu, enter -1 into the field.\n");
            do
            {
                // Call Read Registration helper
                string registration = ReadInput.ReadRegistration(menu);

                // Check if the ID given is an existing vehicle
                List<Vehicle> deleteList = fleetList.GetFleet();
                didWork = deleteList.Exists(item => item.registration == registration);
                if (didWork)
                {
                    Vehicle deleteVehicle = fleetList.GetVehicle(registration);
                    // Output the selected vehicle information
                    Console.WriteLine("\n" + deleteVehicle);
                    Console.Write("Are you sure you wish to delete this vehicle? (y/n): ");
                    string userInput = Console.ReadLine();
                    if (userInput == "y" || userInput == "Y")
                    {
                        bool isDeleted = fleetList.RemoveVehicle(registration);
                        if (isDeleted)
                        {
                            Console.WriteLine("Vehicle successfully deleted. Press Enter to continue.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Returning to Fleet Menu. Press Enter to Continue.");
                            Console.ReadLine();
                            Menu.FleetMenu();
                        }
                    }
                    else if (userInput == "n" || userInput == "N")
                    {
                        Console.WriteLine("Vehicle not deleted. Press Enter to continue.");
                        Console.ReadLine();
                        Menu.FleetMenu();
                    }
                    else
                    {
                        Console.WriteLine("Wrong input expected. Returning to Fleet Menu, press Enter to continue.");
                        Console.ReadLine();
                        Menu.FleetMenu();
                    }
                }
            } while (!didWork);

            // Update fleet list to add deleted vehicle
            Menu.fleetList = fleetList;
        } // End of DeleteVehicle

        // Helper method for Renting Vehicle
        // Request a rental time (in days) and calculate total cost accordingly.
        public static void RentVehicle()
        {
            bool didWork;
            string menu = "rental";
            string registration;
            string prompt;
            Console.WriteLine("\n\nPlease enter a vehicle registration to rent."
                    + "\nAfterward, enter a customer who will be renting that vehicle."
                    + "\nIf You wish to go back to the previous menu, enter -1 into the field.\n");
            do
            {
                int ID;
                // Get registration
                do
                {
                    // Call Read Registration helper
                    registration = ReadInput.ReadRegistration(menu);

                    // Check if the registration given not an existing rented vehicle
                    List<Vehicle> checkList = fleetList.GetFleet(false);
                    didWork = checkList.Exists(item => item.registration == registration);
                    // If registration does not exist in unrented list (vehicle is rented/does not exist)
                    if (!didWork)
                    {
                        try
                        {
                            int rentedID = fleetList.RentedBy(registration);
                            // Vehicle does not exist
                            if (rentedID == -1)
                            {
                                Console.WriteLine("Registration not found, please try again!");
                            }
                            // Vehicle already rented by a customer
                            else
                            {
                                throw new CurrentlyRentedException($"Vehicle already rented to customer {rentedID}, please try again!");
                            }
                        }
                        catch (CurrentlyRentedException e)
                        {
                            Console.WriteLine(e.Message);
                            didWork = false;
                        }
                    }
                } while (!didWork);

                // Get ID
                do
                {
                    // Call ReadID helper
                    prompt = "ID*: ";
                    ID = ReadInput.ReadInt(prompt, menu);

                    // Check if the ID given is an existing customer
                    List<Customer> checkList = customerList.GetCustomers();
                    didWork = checkList.Exists(item => item.customerID == ID);
                    if (!didWork)
                    {
                        Console.WriteLine("ID not found, please try again!");
                    }
                    // Check if customer is already renting a vehicle
                    else
                    {
                        try
                        {
                            if (fleetList.IsRenting(ID))
                            {
                                throw new CurrentlyRentedException($"Customer {ID} already renting a vehicle!");
                            }
                        }
                        catch(CurrentlyRentedException e)
                        {
                            Console.WriteLine(e.Message);
                            didWork = false;
                        }
                    }
                } while (!didWork);
                // Calculate total cost of vehicle
                prompt = "Amount of Days Vehicle is Rented*: ";
                Console.Write("\nEnter amount of days vehicle will be rented for: ");
                double amtDays = ReadInput.ReadInt(prompt, menu);
                Vehicle selectedVehicle = fleetList.GetVehicle(registration);
                double totalCost = amtDays * selectedVehicle.dailyRate;
                string totalMoney = totalCost.ToString("C");

                didWork = fleetList.RentVehicle(registration, ID);
                // If for whatever reason above exception handling failed output this
                if (!didWork)
                {
                    Console.WriteLine("Critical error renting vehicle, please try again.");
                }
                else
                {
                    Console.WriteLine($"Vehicle {registration} successfully rented to customer {ID}.");
                    Console.WriteLine("Total cost for rental is: " + totalMoney);
                    Console.WriteLine("Press Enter to continue. ");
                    Console.ReadLine();
                }
            } while (!didWork);
        } // End of RentVehicle
    }
}
