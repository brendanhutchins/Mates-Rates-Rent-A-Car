using System;
using System.Collections.Generic;
using System.Linq;
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
    /// This class was created to handle multiple instances of input 
    /// calling from multiple classes to declutter the program.
    /// 
    ///
    /// Author Brendan Hutchins May 2020
    ///
    ///
    /// </summary>
    public class ReadInput
    {
        // Handle non-year/non-menu int input
        public static int ReadInt(string prompt, string menu)
        {
            bool isValid;
            string userInput;
            int ID;

            do
            {
                Console.Write(prompt);
                userInput = Console.ReadLine();
                isValid = int.TryParse(userInput, out ID);
                if (!isValid || ID < 0)
                {
                    if (ID != -1)
                    {
                        isValid = false;
                        Console.Write("Incorrect input detected, please try again.\n");
                    }
                    else
                    {
                        if (menu == "customer")
                        {
                            Menu.CustomerMenu();
                        }
                        else if (menu == "fleet")
                        {
                            Menu.FleetMenu();
                        }
                        else if (menu == "rental")
                        {
                            Menu.RentalMenu();
                        }
                    }
                }
            } while (!isValid);

            return ID;
        }

        // Handle string input
        public static string ReadString(string prompt, string menu)
        {
            bool isFalse;
            string userInput;

            Console.Write(prompt);
            do
            {
                userInput = Console.ReadLine();
                isFalse = userInput.Any(char.IsDigit);
                if (isFalse || string.IsNullOrEmpty(userInput))
                {
                    if (userInput == "-1")
                    {
                        if (menu == "customer")
                        {
                            Menu.CustomerMenu();
                        }
                        else if (menu == "fleet")
                        {
                            Menu.FleetMenu();
                        }
                        else if (menu == "rental")
                        {
                            Menu.RentalMenu();
                        }
                    }
                    else if (prompt == "Model*: ")
                    {
                        isFalse = false;
                    }
                    else
                    {
                        isFalse = true;
                        Console.Write("Incorrect input detected, please try again.\n");
                        Console.Write(prompt);
                    }
                }
            } while (isFalse);

            char[] a = userInput.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            userInput = new string(a);

            return userInput;
        }

        // Handle any bool input
        public static bool ReadBool(string prompt)
        {
            bool didWork = false;
            bool carPart = false;
            do
            {
                Console.Write(prompt);
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "y":
                    case "Y":
                        didWork = true;
                        carPart = true;
                        break;
                    case "n":
                    case "N":
                        didWork = true;
                        carPart = false;
                        break;
                    case "-1":
                        Menu.FleetMenu();
                        break;
                    default:
                        Console.WriteLine("Wrong input detected, please try again.");
                        didWork = false;
                        break;
                }
            } while (!didWork);

            return carPart;
        }

        // Handle Registration
        public static string ReadRegistration(string menu)
        {
            bool isValid;
            string userInput;

            do
            {
                Console.Write("Registration*: ");
                userInput = Console.ReadLine();
                // Check if user wants to go back
                if (userInput == "-1")
                {
                    if (menu == "fleet")
                    {
                        Menu.FleetMenu();
                    }
                    else if (menu == "rental")
                    {
                        Menu.RentalMenu();
                    }
                }

                if (userInput.Length == 6)
                {
                    // Break the registration in half into numbers and letters groups
                    string numbers = userInput.Substring(0, 3);
                    string letters = userInput.Substring(3, 3);

                    isValid = Int32.TryParse(numbers, out int result);
                    if (isValid != false)
                    {
                        isValid = letters.All(Char.IsLetter);
                        if (isValid == false)
                        {
                            Console.WriteLine("Incorrect format, example format for registration is '123ABC'");
                        }
                    }
                    else { Console.WriteLine("Incorrect format, example format for registration is '123ABC'"); }   
                }
                else
                {
                    isValid = false;
                    Console.WriteLine("Incorrect length, example format for registration is '123ABC'");
                }
            } while (!isValid);

            string registration = userInput.ToUpper();

            return registration;
        }

        // Handle vehicle year
        public static int ReadYear()
        {
            bool isValid;
            string userInput;
            int year;

            do
            {
                Console.Write("Year*: ");
                userInput = Console.ReadLine();
                isValid = int.TryParse(userInput, out year);
                if (!isValid || year < 1908 || year > 2021)
                {
                    if (year != -1)
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

            return year;
        }

        // Handle converting a string to capitals
        public static string ToCapitals(string prompt, string menu)
        {
            string input = ReadString(prompt, menu);

            // Make first character uppercase, all others lowercase
            char[] a = input.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            for (int i = 1; i < a.Length; i++)
            {
                a[i] = char.ToLower(a[i]);
            }
            input = new string(a);
            return input;
        }

        // Handle menu input and navigation
        public static int ReadOption(int optionAmt, string menu)
        {
            ConsoleKeyInfo userChoice;
            bool isValid;
            ConsoleKey maxKey;
            ConsoleKey maxNumPad;

            if (optionAmt == 3)
            {
                maxKey = ConsoleKey.D3;
                maxNumPad = ConsoleKey.NumPad3;
            }
            else
            {
                maxKey = ConsoleKey.D4;
                maxNumPad = ConsoleKey.NumPad4;
            }

            do
            {
                userChoice = Console.ReadKey();

                if (userChoice.Key != ConsoleKey.Escape && userChoice.Key != ConsoleKey.Backspace)
                {
                    if (userChoice.Key < ConsoleKey.D1 || userChoice.Key > maxKey)
                    {
                        if (userChoice.Key < ConsoleKey.NumPad1 || userChoice.Key > maxNumPad)
                        {
                            Console.WriteLine("\nYou did not enter a correct option.\n\nPlease try again.");
                            Console.Write(menu);
                            isValid = false;
                        }
                        else { isValid = true; }
                    }
                    else { isValid = true; }
                }
                else { isValid = true; }

            } while (!isValid);

            if (userChoice.Key == ConsoleKey.Escape)
            {
                Menu.ExitProgram();
            }

            else if (userChoice.Key == ConsoleKey.Backspace)
            {
                Menu.MainMenu();
            }

            switch (userChoice.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return 1;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return 2;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return 3;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    return 4;
                default:
                    Console.WriteLine("Invalid Number detected");
                    return 0;
            }
        }
    }
}
