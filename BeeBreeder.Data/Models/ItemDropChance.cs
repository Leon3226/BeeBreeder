using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class ItemDropChance
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int SpecieId { get; set; }
        public bool IsSpecialization { get; set; }
        public double Chance { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual Specie Specie { get; set; } = null!;
    }
}
