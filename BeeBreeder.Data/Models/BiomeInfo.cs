using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class BiomeInfo
    {
        public string Name { get; set; } = null!;
        public int Temperature { get; set; }
        public int Humidity { get; set; }
    }
}
