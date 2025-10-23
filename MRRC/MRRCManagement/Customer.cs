using MRRC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Customer class handles the creation of customer objects.
    // Deals with customerID, title, firstName, lastName, gender, and date of birth
    public class Customer
    {
        public int customerID { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Gender gender { get; set; }
        public DateTime DOB { get; set; }

        // This constructor should construct a customer with the provided attributes.
        public Customer(int ID, string title, string firstName, string lastName, Gender gender, DateTime DOB)
        {
            customerID = ID;
            this.title = title;
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.DOB = DOB;
        }

        // This method should return a CSV representation of the customer that is consistent
        // with the provided data files.
        public string ToCSVString()
        {
            string date = DOB.ToString("dd/MM/yyyy");
            return $"{customerID},{title},{firstName},{lastName},{gender},{date}";
        }

        // This method should return a string representation of the attributes of the customer.
        // This is used by the table class
        public override string ToString()
        {
            string date = DOB.ToString("dd/MM/yyyy");
            //string returnString = $"│{customerID,-4}│{title,-5}│{firstName,-10}│{lastName,-14}│{gender,-7}│{date,-7}│";
            string returnString = $"{customerID}│{title}│{firstName}│{lastName}│{gender}│{date}";
            return returnString;
        }
    }
}
