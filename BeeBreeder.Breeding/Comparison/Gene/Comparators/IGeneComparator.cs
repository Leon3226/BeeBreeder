using System.Collections.Generic;

namespace BeeBreeder.Breeding.Comparison.Gene.Comparators
{
    public interface IGeneComparator
    {
        Dictionary<string, IGeneValueComparator> Comparators { get; }
        Comparison Compare(object first, object second, string propertyName);
    }
}
