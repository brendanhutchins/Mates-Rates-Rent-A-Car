using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Abstract class that has default values that are changed by each instance of
    // VehicleGrade (Includes: Economy, Luxury, Commercial, and Family). Also adds
    // Vehicle attribute list to be used by Search.
    public abstract class Vehicle
    {
        public string registration;
        public VehicleGrade grade;
        public string make;
        public string model;
        public int year;

        public int numSeats;
        public TransmissionType transmission;
        public FuelType fuel;
        public bool GPS;
        public bool sunRoof;
        public double dailyRate;
        public string colour;

        // This constructor provides values for all parameters of the vehicle.
        public Vehicle(string registration, VehicleGrade grade, string make, string model, 
            int year, int numSeats, TransmissionType transmission, FuelType fuel, 
            bool GPS, bool sunRoof, double dailyRate, string colour)
        {
            this.registration = registration;
            this.grade = grade;
            this.make = make;
            this.model = model;
            this.year = year;
            this.numSeats = numSeats;
            this.transmission = transmission;
            this.fuel = fuel;
            this.GPS = GPS;
            this.sunRoof = sunRoof;
            this.dailyRate = dailyRate;
            this.colour = colour;
        }

        // This constructor provides only the mandatory parameters of the vehicle. Others are set based
        // on the defaults of each VehicleGrade class (Economy, Luxury, Commercial, and Family)
        public Vehicle(string registration, VehicleGrade grade, string make, string model, int year)
        {
            this.registration = registration;
            this.grade = grade;
            this.make = make;
            this.model = model;
            this.year = year;

            numSeats = 4;
            transmission = TransmissionType.Manual;
            fuel = FuelType.Petrol;
            GPS = false;
            sunRoof = false;
            dailyRate = 50.00;
            colour = "Black";
        }
        
        // Empty constructor for ModifyVehicle to have a temporary vehicle instance
        public Vehicle()
        {
        }

        // This method should return a CSV representation of the vehicle that is consistent
        // with the provided data files.
        public string ToCSVString()
        {
            return $"{registration},{grade},{make},{model},{year},{numSeats},{transmission},{fuel},{GPS},{sunRoof},{dailyRate},{colour}";
        }

        // This method should return a string representation of the attributes of the vehicle.
        // This is used by the table class
        public override string ToString()
        {
            string GPSString;
            string sunRoofString;
            if (GPS == true)
            {
                GPSString = "GPS";
            }
            else
            {
                GPSString = "-";
            }
            if (sunRoof == true)
            {
                sunRoofString = "sunroof";
            }
            else
            {
                sunRoofString = "-";
            }
            return $"{registration}│{grade}│{make}│{model}│{year}│{numSeats}│{transmission}│{fuel}│{GPSString}│{sunRoofString}│{dailyRate}│{colour}";
        }

        // This method should return a list of strings which represent each attribute.
        // Each string is corrected to be all uppercase to be easily searched.
        public List<string> GetAttributeList()
        {
            List<string> attributeList = new List<string>();

            attributeList.Add(registration);
            attributeList.Add(grade.ToString().ToUpper());
            attributeList.Add(make.ToUpper().Replace(" ", ""));
            attributeList.Add(model.ToUpper().Replace(" ", ""));
            attributeList.Add(year.ToString());
            attributeList.Add(numSeats.ToString() + "-SEATER");
            attributeList.Add(transmission.ToString().ToUpper());
            attributeList.Add(fuel.ToString().ToUpper());
            if (GPS == true)
            {
                string GPSString = "GPS";
                attributeList.Add(GPSString);
            }
            if (sunRoof == true)
            {
                string sunRoofString = "SUNROOF";
                attributeList.Add(sunRoofString);
            }
            attributeList.Add(colour.ToUpper());

            return attributeList;
        }
    }
}
