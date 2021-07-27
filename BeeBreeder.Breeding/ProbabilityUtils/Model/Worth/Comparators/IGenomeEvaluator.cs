using System.Collections.Generic;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators.Functions;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators
{
    public interface IGenomeEvaluator
    {
        double Evaluate(Genotype genotype);
    }
}