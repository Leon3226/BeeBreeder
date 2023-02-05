using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class Flower
    {
        public Flower()
        {
            TransposerFlowers = new HashSet<TransposerFlower>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ModId { get; set; }

        public virtual ICollection<TransposerFlower> TransposerFlowers { get; set; }
    }
}
