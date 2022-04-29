using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.StatsCrawler
{
    internal class BeeParsingData
    {
        public string? Name { get; set; }
        public string? LatinName { get; set; }
        public string? ImageUrl { get; set; }
        public string? WikiUrl { get; set; }
        public string? DiscoveredBy { get; set; }
        public string? Mod { get; set; }
        public string? Branch { get; set; }
        public string? Description { get; set; }
        public string? Speed { get; set; }
        public string? Lifespan { get; set; }
        public string? Fertility { get; set; }
        public string? Pollination { get; set; }
        public string? Territory { get; set; }
        public string? Flowers { get; set; }
        public string? Effect { get; set; }
        public string? CaveDwelling { get; set; }
        public string? Nocturnal { get; set; }
        public string? RainTolerant { get; set; }
        public string? Temperature { get; set; }
        public string? TempTolerance { get; set; }
        public string? Humidity { get; set; }
        public string? HumidTolerance { get; set; }
        public List<ProductChance> Products { get; set; } = new();
        public List<ProductChance> SpecialtyProducts { get; set; } = new();
        public List<Mutation> Mutations { get; set; } = new();
        public List<string> Notes { get; set; } = new();
    }
}
