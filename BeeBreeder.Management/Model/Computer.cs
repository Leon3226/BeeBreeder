using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Management.Model
{
    public class Computer
    {
        public string Identifier { get; set; }

        public Transposer[] Trasposers;

        public override string ToString()
        {
            return Identifier;
        }

    }
}
