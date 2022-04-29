using System.Collections.Generic;

namespace BeeBreeder.Breeding.Comparison.Gene.Priority
{
    public interface IPriorityProvider
    {
        IDictionary<string, int> Priorities { get; }
    }
}
