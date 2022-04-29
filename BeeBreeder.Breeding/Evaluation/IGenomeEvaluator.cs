using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Evaluation
{
    public interface IGenomeEvaluator
    {
        double Evaluate(Genotype genotype);
    }
}