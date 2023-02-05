using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class MutationChancesHived
    {
        public int FirstId { get; set; }
        public int SecondId { get; set; }
        public int ResultId { get; set; }
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public string ResultName { get; set; } = null!;
        public int? FirstModId { get; set; }
        public int? SecondModId { get; set; }
        public int? ResultModId { get; set; }
        public double Chance { get; set; }
    }
}
