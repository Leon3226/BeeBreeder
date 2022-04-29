using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class LootItem
    {
        public int ItemId { get; set; }
        public int SpecieId { get; set; }
        public bool IsSpecialization { get; set; }
        public double Chance { get; set; }
    }
}
