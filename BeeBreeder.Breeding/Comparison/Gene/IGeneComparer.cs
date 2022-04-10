using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Comparison.Gene
{
    public interface IGeneComparer
    {
        ProbabilityUtils.Model.Worth.Comparison Compare(IGene gene1, IGene gene2, string property, BreedingTarget target = null);
        int Numeric(IGene gene, BreedingTarget target = null);
    }
}
