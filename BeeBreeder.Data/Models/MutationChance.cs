using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class MutationChance
    {
        public int Id { get; set; }
        public int? FirstId { get; set; }
        public int? SecondId { get; set; }
        public int ResultId { get; set; }
        public double Chance { get; set; }

        public virtual Specie? First { get; set; }
        public virtual Specie Result { get; set; } = null!;
        public virtual Specie? Second { get; set; }
    }
}
