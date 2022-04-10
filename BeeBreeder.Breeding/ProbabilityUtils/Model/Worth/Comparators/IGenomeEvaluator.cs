using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators
{
    public interface IGenomeEvaluator
    {
        double Evaluate(Genotype genotype);
    }
}