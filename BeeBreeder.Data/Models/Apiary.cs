using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class Apiary
    {
        public Apiary()
        {
            ApiaryComputers = new HashSet<ApiaryComputer>();
            ApiaryMods = new HashSet<ApiaryMod>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string UserId { get; set; } = null!;

        public virtual ICollection<ApiaryComputer> ApiaryComputers { get; set; }
        public virtual ICollection<ApiaryMod> ApiaryMods { get; set; }
    }
}
