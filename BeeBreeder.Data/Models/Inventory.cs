using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class Inventory
    {
        public string TransposerId { get; set; } = null!;
        public int Side { get; set; }
        public string InGameId { get; set; } = null!;
        public string InGameLabel { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ItemUnderId { get; set; }

        public virtual Item? ItemUnder { get; set; }
        public virtual TransposerDatum Transposer { get; set; } = null!;
    }
}
