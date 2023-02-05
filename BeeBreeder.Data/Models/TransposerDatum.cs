using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class TransposerDatum
    {
        public TransposerDatum()
        {
            Inventories = new HashSet<Inventory>();
            TransposerFlowers = new HashSet<TransposerFlower>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Biome { get; set; }
        public bool? Roofed { get; set; }
        public int ComputerId { get; set; }

        public virtual ApiaryComputer Computer { get; set; } = null!;
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<TransposerFlower> TransposerFlowers { get; set; }
    }
}
