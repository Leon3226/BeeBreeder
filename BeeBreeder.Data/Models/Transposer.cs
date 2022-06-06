using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class Transposer
    {
        public Transposer()
        {
            TransposerFlowers = new HashSet<TransposerFlower>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Biome { get; set; }
        public bool? Roof { get; set; }

        public virtual ICollection<TransposerFlower> TransposerFlowers { get; set; }
    }
}
