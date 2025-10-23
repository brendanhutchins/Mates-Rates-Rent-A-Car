using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Class that handles fleet, vehicle, and rental functionality
    public class Fleet
    {
        private List<Vehicle> fleet;
        private Dictionary<string, int> rentals;
        private string fleetFile;
        private string rentalsFile;
        public SortedList attributeSets;

        // If there is no fleet file at the specified location, this constructor constructors
        // an empty fleet and empty rentals. Otherwise it loads the fleet and rentals from the
        // specified files.
        public Fleet(string fleetFile, string rentalsFile)
        {
            // Initialize attributeSet sorted list
            attributeSets = new SortedList();

            // Fleet File
            if (!File.Exists(fleetFile))
            {
                this.fleetFile = @"..\..\..\Data\fleet.csv";
                Console.WriteLine("\nFleet file created at Data/fleet.csv");
                fleet = new List<Vehicle>();
            }
            else
            {
                this.fleetFile = fleetFile;
                fleet = new List<Vehicle>();
                LoadVehiclesFromFile();
            }
            
            // Rental File
            if (!File.Exists(rentalsFile))
            {
                this.rentalsFile = @"..\..\..\Data\rentals.csv";
                Console.WriteLine("\nRentals file created at Data/rentals.csv");
                rentals = new Dictionary<string, int>();
            }    
            else
            {
                this.rentalsFile = rentalsFile;
                rentals = new Dictionary<string, int>();
                LoadRentalsFromFile();
            }

            
        }

        // public reference for rentals dictionary
        public Dictionary<string,int> Rentals
        {
            get
            {
                return rentals;
            }

        }

        // Adds the provided vehicle to the fleet assuming the vehicle registration does not
        // already exist. It returns true if the add was successful (the registration did not
        // already exist in the fleet), and false otherwise.
        public bool AddVehicle(Vehicle vehicle)
        {
           
            if (!fleet.Exists(x => x.registration == vehicle.registration))
            {
                fleet.Add(vehicle);
                InsertVehicle(vehicle);
                return true;
            }
            else
            {
                return false;
            }
        }


        // This method removes the vehicle with the provided rego from the fleet if it is not
        // currently rented. It returns true if the removal was successful and false otherwise.
        public bool RemoveVehicle(string registration)
        {
            try
            {
                List<Vehicle> fullList = GetFleet();
                Vehicle removableVehicle = fleet.Find(x => x.registration == registration);
                // Check if vehicle is currently rented
                if (IsRented(registration) == false)
                {
                    fullList.Remove(removableVehicle);
                    return true;
                }
                else
                {
                    throw new CurrentlyRentedException("\nVehicle selected is currently rented. "
                                                       + "Please return vehicle before deleting vehicle.");
                }
            }
            catch (CurrentlyRentedException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // This method returns a vehicle that matches the given registration
        public Vehicle GetVehicle(string registration)
        {
            List<Vehicle> fullList = GetFleet();
            Vehicle foundVehicle = fullList.Find(item => item.registration == registration);
            return foundVehicle;
        }

        // This method returns the fleet (as a list of Vehicles).
        public List<Vehicle> GetFleet()
        {
            return fleet;
        }

        // This method returns a subset of vehicles in the fleet depending on whether they are
        // currently rented. If rented is true, this method returns all rented vehicles. If it
        // false, this method returns all not yet rented vehicles.
        public List<Vehicle> GetFleet(bool rented)
        {
            // create temporary fleet to not compromise real fleet
            List<Vehicle> rentalFleet = new List<Vehicle>();

            for (int i = 0; i < fleet.Count; i++)
            {
                if (rented)
                {
                    // Add all rented vehicles to list
                    if (rentals.ContainsKey(fleet[i].registration))
                    {
                        rentalFleet.Add(fleet[i]);
                        //Console.WriteLine("Vehicle " + fleet[i].registration + " added to rented list.");
                    }
                }
                else
                {
                    // Add all nonrented vehicles to list
                    if (!rentals.ContainsKey(fleet[i].registration))
                    {
                        rentalFleet.Add(fleet[i]);
                        //Console.WriteLine("Vehicle " + fleet[i].registration + " added to nonrented List.");
                    }
                }
            }
            return rentalFleet;
        }

        // This method returns whether the vehicle with the provided registration is currently
        // being rented.
        public bool IsRented(string registration)
        {
            if (rentals.ContainsKey(registration))
                return true;
            else
                return false;
        }

        // This method returns whether the customer with the provided customer ID is currently
        // renting a vehicle.
        public bool IsRenting(int customerID)
        {
            if (rentals.ContainsValue(customerID))
                return true;
            else
                return false;
        }

        // This method returns the customer ID of the current renter of the vehicle. If it is
        // rented by no one, it returns -1.
        public int RentedBy(string registration)
        {
            if (rentals.TryGetValue(registration, out int value))
                return value;
            else
                return -1;
        }

        // This method rents the vehicle with the provided registration to the customer with
        // the provided ID, if the vehicle is not currently being rented. It returns true if
        // the rental was possible, and false otherwise.
        public bool RentVehicle(string registration, int customerID)
        {
            if (!rentals.ContainsKey(registration))
            {
                rentals.Add(registration, customerID);
                return true;
            }
            else
            {
                return false;
            }
        }

        // This method returns a vehicle. If the return is successful (it was currently being
        // rented) it returns the customer ID of the customer who was renting it, otherwise it
        // returns -1.
        public int ReturnVehicle(string registration)
        {
            if (rentals.TryGetValue(registration, out int customerID))
            {
                rentals.Remove(registration);
                return customerID;
            }
            else
            {
                return -1;
            }
        }

        // This method saves the current list of vehicles to file.
        public void SaveVehiclesToFile()
        {
            using (StreamWriter writer = new StreamWriter(fleetFile))
            {
                writer.WriteLine("Registration,Grade,Make,Model,Year,NumSeats,Transmission,Fuel,GPS,SunRoof,DailyRate,Colour");
                foreach (Vehicle vehicle in fleet)
                {
                    writer.WriteLine(vehicle.ToCSVString());
                }
            }
        }

        // This method saves the current list of rentals to file.
        public void SaveRentalsToFile()
        {
            using (StreamWriter writer = new StreamWriter(rentalsFile))
            {
                writer.WriteLine("Registration,ID");
                foreach (KeyValuePair<string, int> rental in rentals)
                {
                    writer.WriteLine($"{rental.Key},{rental.Value}");
                }
            }
        }

        // This method loads the list of vehicles from the file.
        private void LoadVehiclesFromFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(fleetFile))
                {
                    // Don't throw the header line to the fleet list
                    string headerLine = reader.ReadLine();
                    // Console.WriteLine(headerLine);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] fields = line.Split(',');

                        // Change all strings to their respective values in Customer
                        string registration = fields[0];
                        VehicleGrade grade = (VehicleGrade)Enum.Parse(typeof(VehicleGrade), fields[1]);
                        string make = fields[2];
                        string model = fields[3];
                        int year = Convert.ToInt32(fields[4]);
                        int numSeats = Convert.ToInt32(fields[5]);
                        TransmissionType transmission = (TransmissionType)Enum.Parse(typeof(TransmissionType), fields[6]);
                        FuelType fuel = (FuelType)Enum.Parse(typeof(FuelType), fields[7]);
                        bool GPS = Convert.ToBoolean(fields[8]);
                        bool sunRoof = Convert.ToBoolean(fields[9]);
                        double dailyRate = Convert.ToDouble(fields[10]);
                        string colour = fields[11];

                        // Add customers to customer list
                        switch (grade)
                        {
                            case VehicleGrade.Economy:
                                AddVehicle(new Economy(registration, grade, make, model, year, numSeats, transmission, fuel, GPS, sunRoof, dailyRate, colour));
                                break;
                            case VehicleGrade.Family:
                                AddVehicle(new Family(registration, grade, make, model, year, numSeats, transmission, fuel, GPS, sunRoof, dailyRate, colour));
                                break;
                            case VehicleGrade.Luxury:
                                AddVehicle(new Luxury(registration, grade, make, model, year, numSeats, transmission, fuel, GPS, sunRoof, dailyRate, colour));
                                break;
                            case VehicleGrade.Commercial:
                                AddVehicle(new Commercial(registration, grade, make, model, year, numSeats, transmission, fuel, GPS, sunRoof, dailyRate, colour));
                                break;
                        }
                    }
                }
            }
            // Usually fires if file is open in other location
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press enter to close this MRRC program.");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        // This method loads the list of rentals from the file.
        private void LoadRentalsFromFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(rentalsFile))
                {
                    // Don't throw the header line to the fleet list
                    string headerLine = reader.ReadLine();
                    // Console.WriteLine(headerLine);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] fields = line.Split(',');

                        // Change all strings to their respective values in Customer
                        string registration = fields[0];
                        int ID = Convert.ToInt32(fields[1]);

                        // Add rentals to rental list
                        rentals.Add(registration, ID);
                    }
                }
            }
            // Usually fires if file is open in other location
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press enter to close this MRRC program.");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        // Inspired from Shlomo Geva's code, method to insert a vehicle into AttributeSet's hashset
        // to include each matching vehicle registration for use by Search function
        private void InsertVehicle(Vehicle vehicle)
        {
            int idx;
            HashSet<string> hs;
            List<string> attributeList;

            attributeList = vehicle.GetAttributeList();
            // Check if the item given is in the attributeSet list
            foreach (string item in attributeList)
            {
                if (!attributeSets.ContainsKey(item))
                    attributeSets.Add(item, new HashSet<string>());
            }
            // If Registration set is found
            idx = attributeSets.IndexOfKey(vehicle.registration);
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
            // If Grade set is found
            idx = attributeSets.IndexOfKey(vehicle.grade.ToString().ToUpper());
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
            // If Make set is found - Include spaces
            idx = attributeSets.IndexOfKey(vehicle.make.ToUpper().Replace(" ", ""));
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
            // If Model set is found - Include spaces
            idx = attributeSets.IndexOfKey(vehicle.model.ToUpper().Replace(" ", ""));
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
            // If Year set is found
            idx = attributeSets.IndexOfKey(vehicle.year.ToString());
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
            // If NumSeats set is found
            idx = attributeSets.IndexOfKey(vehicle.numSeats.ToString() + "-SEATER");
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
            // If Transmission set is found
            idx = attributeSets.IndexOfKey(vehicle.transmission.ToString().ToUpper());
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
            // If Fuel set is found
            idx = attributeSets.IndexOfKey(vehicle.fuel.ToString().ToUpper());
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
            // If GPS set is found
            if (vehicle.GPS == true)
            {
                string GPSString = "GPS";
                idx = attributeSets.IndexOfKey(GPSString);
                if (idx >= 0)
                {
                    hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                    hs.Add(vehicle.registration); // Add to set
                    attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
                }
            }
            // If SunRoof set is found
            if (vehicle.sunRoof == true)
            {
                string sunRoofString = "SUNROOF";
                idx = attributeSets.IndexOfKey(sunRoofString);
                if (idx >= 0)
                {
                    hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                    hs.Add(vehicle.registration); // Add to set
                    attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
                }
            }
            // If Colour set is found
            idx = attributeSets.IndexOfKey(vehicle.colour.ToUpper());
            if (idx >= 0)
            {
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // Get set
                hs.Add(vehicle.registration); // Add to set
                attributeSets.SetByIndex(idx, hs); // Save set (replaces old set)
            }
        }
    }
}
