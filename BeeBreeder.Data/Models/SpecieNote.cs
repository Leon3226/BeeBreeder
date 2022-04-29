using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class SpecieNote
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int SpecieId { get; set; }

        public virtual Specie Specie { get; set; } = null!;
    }
}
