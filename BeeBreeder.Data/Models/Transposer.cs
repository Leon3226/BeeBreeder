using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeBreeder.Data.Models
{
    public partial class Transposer
    {
        public Transposer()
        {
            TransposerFlowers = new HashSet<TransposerFlower>();
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Biome { get; set; }
        public bool? Roofed { get; set; }
        public int ComputerId { get; set; }

        public virtual ApiaryComputer Computer { get; set; } = null!;
        public virtual ICollection<TransposerFlower> TransposerFlowers { get; set; }
    }
}
