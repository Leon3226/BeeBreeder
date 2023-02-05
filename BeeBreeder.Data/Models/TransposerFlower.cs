using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class TransposerFlower
    {
        public int Id { get; set; }
        public string TransposerId { get; set; } = null!;
        public int FlowerId { get; set; }

        public virtual Flower Flower { get; set; } = null!;
        public virtual TransposerDatum Transposer { get; set; } = null!;
    }
}
