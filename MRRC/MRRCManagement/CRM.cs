using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using MRRC;
using System.Globalization;

namespace MRRCManagement
{
    // Class that handles customer functionality
    public class CRM
    {
        private List<Customer> customers;
        private string crmFile;

        // If there is no CRM file at the specified location, this constructor constructors an
        // empty CRM with no customers. Otherwise it loads the customers from the specified file
        // (see the assignment specification).
        public CRM(string crmFile)
        {
            // If file does not exist
            if (!File.Exists(crmFile))
            {
                this.crmFile = @"..\..\..\Data\customers.csv";
                Console.WriteLine("\nCustomer File created at Data/customers.csv");
                customers = new List<Customer>();
            } 
            else
            {
                this.crmFile = crmFile;
                customers = new List<Customer>();
                LoadFromFile();
            } 
        }

        // This method adds the provided customer to the customer list if the customer ID doesn’t
        // already exist in the CRM. It returns true if the addition was successful (the customer
        // ID didn’t already exist in the CRM) and false otherwise.
        public bool AddCustomer(Customer customer)
        {
            if (!customers.Exists(x => x.customerID == customer.customerID))
            {
                customers.Add(customer);
                return true;
            }
            else
            {
                return false;
            }
        }

        // This method removes the customer with the provided customer ID from the CRM if they
        // are not currently renting a vehicle. It returns true if the removal was successful,
        // otherwise it returns false.
        public bool RemoveCustomer(int ID, Fleet fleet)
        {
            try
            {
                List<Customer> fullList = GetCustomers();
                Customer removableCustomer = GetCustomer(ID);
                if (fleet.IsRenting(ID) == false)
                {
                    fullList.Remove(removableCustomer);
                    return true;
                }
                else
                {
                    throw new CurrentlyRentedException("\nCustomer selected is currently renting a vehicle. "
                                                       + "Please return vehicle before deleting customer.");
                }
            }
            catch (CurrentlyRentedException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // This method returns the list of current customers.
        public List<Customer> GetCustomers()
        {
            return customers;
        }

        // This method returns the customer who matches the provided ID.
        public Customer GetCustomer(int ID)
        {
            List<Customer> fullList = GetCustomers();
            Customer foundCustomer = fullList.Find(item => item.customerID == ID);
            return foundCustomer;
        }

        // This method saves the current state of the CRM to file.
        public void SaveToFile()
        {
            using (StreamWriter writer = new StreamWriter(crmFile))
            {
                writer.WriteLine("ID,Title,FirstName,LastName,Gender,DOB");
                foreach (Customer customer in customers)
                {
                    writer.WriteLine(customer.ToCSVString());
                }
            }
        }

        // This method loads the state of the CRM from file.
        public void LoadFromFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(crmFile))
                {
                    // Don't throw the header line to the customer list
                    string headerLine = reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] fields = line.Split(',');

                        // Change all strings to their respective values in Customer
                        int ID = Convert.ToInt32(fields[0]);
                        string title = fields[1];
                        string firstName = fields[2];
                        string lastName = fields[3];
                        Gender gender = (Gender)Enum.Parse(typeof(Gender), fields[4]);
                        DateTime DOB = DateTime.Parse(fields[5]);

                        // Add customers to customer list
                        customers.Add(new Customer(ID, title, firstName, lastName, gender, DOB));
                    }
                }
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press enter to close this MRRC program.");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}
