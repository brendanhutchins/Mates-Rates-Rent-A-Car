using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Luxury Vehicle class inheriting from Vehicle abstract class
    public class Luxury : Vehicle
    {
        public Luxury(string registration, VehicleGrade grade, string make, string model,
            int year, int numSeats, TransmissionType transmission, FuelType fuel,
            bool GPS, bool sunRoof, double dailyRate, string colour)
            : base(registration, grade, make, model, year, numSeats,
                   transmission, fuel, GPS, sunRoof, dailyRate, colour)
        {
        }

        public Luxury(string registration, VehicleGrade grade, string make,
            string model, int year) : base(registration, grade, make, model, year)
        {
            // Default values for luxury vehicle
            GPS = true;
            sunRoof = true;
            dailyRate = 120.00;
        }

    }
}
