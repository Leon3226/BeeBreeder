using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Management.Model
{
    public class LoadDataParams
    {
        public bool LoadComputers { get; set; } = true;
        public bool LoadTransposers { get; set; }  = true;
        public bool LoadInventories { get; set; }  = true;
        public bool LoadItems { get; set; } = true;

    }
}
