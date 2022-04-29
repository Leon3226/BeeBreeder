using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class SpecieStat
    {
        public int Id { get; set; }
        public int Speed { get; set; }
        public int Lifespan { get; set; }
        public int Fertility { get; set; }
        public int Pollination { get; set; }
        public string Territory { get; set; } = null!;
        public string Flowers { get; set; } = null!;
        public bool CaveDwelling { get; set; }
        public bool Nocturnal { get; set; }
        public bool RainTolerant { get; set; }
        public int Temperature { get; set; }
        public string TempTolerance { get; set; } = null!;
        public int Humidity { get; set; }
        public string HumidTolerance { get; set; } = null!;
        public int SpecieId { get; set; }
        public bool? IsDefault { get; set; }

        public virtual Specie Specie { get; set; } = null!;
    }
}
