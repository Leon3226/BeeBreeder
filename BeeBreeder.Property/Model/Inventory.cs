using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Property.Model
{
    public class Inventory
    {
        public string TransposerId { get; set; }
        public int Side { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ItemUnderId { get; set; }
        public string InGameId { get; set; }
        public string InGameLabel { get; set; }
    }
}
