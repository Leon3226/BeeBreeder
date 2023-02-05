using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Property.Model
{
    public class Transposer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Roofed { get; set; }
        public string Biome { get; set; }
        public string[] Flowers { get; set; }
    }
}
