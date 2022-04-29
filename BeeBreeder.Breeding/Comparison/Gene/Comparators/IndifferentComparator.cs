namespace BeeBreeder.Breeding.Comparison.Gene.Comparators
{
    public class IndifferentComparator : IGeneValueComparator<object>
    {
        public Comparison Compare(object first, object second)
        {
            return Comparison.Equal;
        }
    }
}
