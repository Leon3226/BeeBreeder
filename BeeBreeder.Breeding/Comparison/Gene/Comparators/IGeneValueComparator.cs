namespace BeeBreeder.Breeding.Comparison.Gene.Comparators
{
    public interface IGeneValueComparator
    {
        Comparison Compare(object first, object second);
    }
    public interface IGeneValueComparator<in T> : IGeneValueComparator
    {
        Comparison Compare(T first, T second);
    }
}
