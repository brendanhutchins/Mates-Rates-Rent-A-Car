using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Family Vehicle class inheriting from Vehicle abstract class
    public class Family : Vehicle
    {
        public Family(string registration, VehicleGrade grade, string make, string model,
            int year, int numSeats, TransmissionType transmission, FuelType fuel,
            bool GPS, bool sunRoof, double dailyRate, string colour)
            : base(registration, grade, make, model, year, numSeats,
                   transmission, fuel, GPS, sunRoof, dailyRate, colour)
        {
        }

        public Family(string registration, VehicleGrade grade, string make,
            string model, int year) : base(registration, grade, make, model, year)
        {
            // Default values for economy vehicle
            dailyRate = 80.00;
        }

    }
}
