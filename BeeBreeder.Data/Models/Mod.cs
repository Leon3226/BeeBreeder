using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class Mod
    {
        public Mod()
        {
            Species = new HashSet<Specie>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Specie> Species { get; set; }
    }
}
