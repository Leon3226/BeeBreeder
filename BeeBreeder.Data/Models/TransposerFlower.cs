using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class TransposerFlower
    {
        public string Id { get; set; } = null!;
        public string TransposerId { get; set; } = null!;
        public string Flower { get; set; } = null!;

        public virtual Transposer Transposer { get; set; } = null!;
    }
}
