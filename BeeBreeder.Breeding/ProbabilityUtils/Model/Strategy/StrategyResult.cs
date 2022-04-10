using System.Collections.Generic;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy
{
    public class StrategyResult
    {
        public List<Species> Species = new();
        public Dictionary<string, List<(Species, IChromosome)>> GenesBest = new();
    }
}