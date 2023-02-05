using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class ApiaryComputer
    {
        public ApiaryComputer()
        {
            TransposerData = new HashSet<TransposerDatum>();
        }

        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string InGameIdentifier { get; set; } = null!;
        public int? ApiaryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? OpenComputersIdentifier { get; set; }

        public virtual Apiary? Apiary { get; set; }
        public virtual ICollection<TransposerDatum> TransposerData { get; set; }
    }
}
