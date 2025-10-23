using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Fire custom exceptions for Rent and Search functionality
    class CurrentlyRentedException : Exception
    {
        public CurrentlyRentedException() { }
        public CurrentlyRentedException(string message)
        : base(message) { }
        public CurrentlyRentedException(string message, Exception inner)
        : base(message, inner) { }
    }
    class ShuntingYardException : Exception
    {
        public ShuntingYardException() { }
        public ShuntingYardException(string message)
        : base(message) { }
        public ShuntingYardException(string message, Exception inner)
        : base(message, inner) { }
    }
}
