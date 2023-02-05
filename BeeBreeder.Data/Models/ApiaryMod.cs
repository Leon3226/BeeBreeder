using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class ApiaryMod
    {
        public int Id { get; set; }
        public int ApiaryId { get; set; }
        public int ModId { get; set; }

        public virtual Apiary Apiary { get; set; } = null!;
        public virtual Mod Mod { get; set; } = null!;
    }
}
