using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators
{
    public interface IGenomeComparator
    {
        Comparison Compare(Genotype left, Genotype right);
    }
}