using System.Collections.Generic;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy
{
    public class StrategyResult
    {
        public List<Species> Species = new();
        public Dictionary<string, List<(Species, IChromosome)>> GenesBest = new();
    }
}