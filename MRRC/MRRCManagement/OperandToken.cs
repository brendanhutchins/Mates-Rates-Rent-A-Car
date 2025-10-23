using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Handle token for vehicle attribute, use its hashset to store instances
    // of matching vehicle registrations.
    class OperandToken : Token
    {
        private string name;
        public HashSet<string> Attributes { get; set; }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = Name;
            }
        }

        public OperandToken(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name.ToString();
        }
    }
}
