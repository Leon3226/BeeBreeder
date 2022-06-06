using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class ComputerBindRequest
    {
        public string ComputerId { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool Resolved { get; set; }
        public bool Failed { get; set; }
        public DateTime Created { get; set; }
        public TimeSpan TimeValid { get; set; }
        public string UserId { get; set; } = null!;
    }
}
