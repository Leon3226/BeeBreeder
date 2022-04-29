using System.Collections.Generic;

namespace BeeBreeder.Breeding.Comparison.Gene.Priority
{
    public class PriorityProvider : IPriorityProvider
    {
        public IDictionary<string, int> Priorities { get; set; } = new Dictionary<string, int>();    
    }
}
