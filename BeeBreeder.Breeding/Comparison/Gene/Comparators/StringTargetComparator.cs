using System.Collections.Generic;
using BeeBreeder.Breeding.Comparison.Gene.Priority;
using BreederComparison = BeeBreeder.Breeding.Comparison.Gene.Comparison;

namespace BeeBreeder.Breeding.Comparison.Gene.Comparators
{
    public class StringTargetComparator : IGeneValueComparator<string>
    {
        private  readonly IPriorityProvider _priorityProvider;
        public StringTargetComparator(IPriorityProvider priorityProvider)
        {
            _priorityProvider = priorityProvider;
        }
            
        public bool UsePriorities = true;
        public BreederComparison Compare(string first, string second)
        {
            if (!UsePriorities)
                return BreederComparison.Equal;

            if (_priorityProvider.Priorities.TryGetValue(first, out int firstValue) && _priorityProvider.Priorities.TryGetValue(second, out int secondValue))
            {
                if (first == second)
                    return Comparison.Equal;
                return firstValue > secondValue ? BreederComparison.Better : BreederComparison.Worse;
            }

            return BreederComparison.Equal;
        }
        
        public BreederComparison Compare(object first, object second)
        {
            return Compare(first as string, second as string);
        }
    }
}
