using BeeBreeder.Common.Model.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Management.Model
{
    public class Transposer
    {
        public string Adress { get; set; }
        public Biome Biome { get; set; }
        public bool Roofed { get; set; }
        public string[] Flowers { get; set; } = new string[0];
        public GameInventory[] Inventories { get; set; } = new GameInventory[6];

        public override string ToString()
        {
            return Adress;
        }
    }
}
