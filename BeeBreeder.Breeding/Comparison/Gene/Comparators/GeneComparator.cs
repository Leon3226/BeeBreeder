using System;
using System.Collections.Generic;

namespace BeeBreeder.Breeding.Comparison.Gene.Comparators
{
    public class GeneComparator : IGeneComparator
    {
        public Dictionary<string, IGeneValueComparator> Comparators { get; } = new();
        public Comparison Compare(object first, object second, string propertyName)
        {
            if (Comparators.TryGetValue(propertyName, out IGeneValueComparator comparator))
                return comparator.Compare(first, second);
            else
                throw new Exception("No appropriate comparator is found");
        }
    }
}
