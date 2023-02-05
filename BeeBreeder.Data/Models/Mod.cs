using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class Mod
    {
        public Mod()
        {
            ApiaryMods = new HashSet<ApiaryMod>();
            Species = new HashSet<Specie>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ApiaryMod> ApiaryMods { get; set; }
        public virtual ICollection<Specie> Species { get; set; }
    }
}
