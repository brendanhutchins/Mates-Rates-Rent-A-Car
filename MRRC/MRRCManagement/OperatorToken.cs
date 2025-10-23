using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Handle operator token for "AND" and "OR". Precedence determines which
    // operator is executed before the other.
    class OperatorToken : Token
    {
        private string name;
        public int Precedence { get; set; }
        
        public OperatorToken(string name, int precedence)
        {
            this.name = name;
            Precedence = precedence;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
