using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class Specie
    {
        public Specie()
        {
            ItemDropChances = new HashSet<ItemDropChance>();
            MutationChanceFirsts = new HashSet<MutationChance>();
            MutationChanceResults = new HashSet<MutationChance>();
            MutationChanceSeconds = new HashSet<MutationChance>();
            SpecieNotes = new HashSet<SpecieNote>();
            SpecieStats = new HashSet<SpecieStat>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? LatinName { get; set; }
        public string? DiscoveredBy { get; set; }
        public int? ModId { get; set; }
        public string? Branch { get; set; }
        public string? Description { get; set; }
        public bool? IsHiveBee { get; set; }

        public virtual Mod? Mod { get; set; }
        public virtual ICollection<ItemDropChance> ItemDropChances { get; set; }
        public virtual ICollection<MutationChance> MutationChanceFirsts { get; set; }
        public virtual ICollection<MutationChance> MutationChanceResults { get; set; }
        public virtual ICollection<MutationChance> MutationChanceSeconds { get; set; }
        public virtual ICollection<SpecieNote> SpecieNotes { get; set; }
        public virtual ICollection<SpecieStat> SpecieStats { get; set; }
    }
}
