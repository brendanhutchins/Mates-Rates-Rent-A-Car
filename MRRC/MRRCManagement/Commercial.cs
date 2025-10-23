using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Commercial Vehicle class inheriting from Vehicle abstract class
    public class Commercial : Vehicle
    {
        public Commercial(string registration, VehicleGrade grade, string make, string model,
            int year, int numSeats, TransmissionType transmission, FuelType fuel,
            bool GPS, bool sunRoof, double dailyRate, string colour)
            : base(registration, grade, make, model, year, numSeats,
                   transmission, fuel, GPS, sunRoof, dailyRate, colour)
        {
        }

        public Commercial(string registration, VehicleGrade grade, string make,
            string model, int year) : base(registration, grade, make, model, year)
        {
            // Default values for commercial vehicle
            fuel = FuelType.Diesel;
            dailyRate = 130.00;
        }
    }
}
