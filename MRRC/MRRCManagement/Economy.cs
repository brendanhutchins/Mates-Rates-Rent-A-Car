using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Economy Vehicle class inheriting from Vehicle abstract class
    public class Economy : Vehicle
    {
        public Economy(string registration, VehicleGrade grade, string make, string model,
            int year, int numSeats, TransmissionType transmission, FuelType fuel,
            bool GPS, bool sunRoof, double dailyRate, string colour)
            : base(registration, grade, make, model, year, numSeats, 
                   transmission, fuel, GPS, sunRoof, dailyRate, colour)
        {
        }

        public Economy(string registration, VehicleGrade grade, string make, 
            string model, int year) : base(registration, grade, make, model, year)
        {
            // Default values for economy vehicle
            transmission = TransmissionType.Automatic;
        }

        // For having a temporary vehicle object in ModifyVehicle in
        // MenuHelpers.
        public Economy()
        {
        }
    }
}
