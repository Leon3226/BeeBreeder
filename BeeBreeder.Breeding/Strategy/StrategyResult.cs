using System.Collections.Generic;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Strategy
{
    public class StrategyResult
    {
        public List<string> Species = new();
        public Dictionary<string, List<(string, IChromosome)>> GenesBest = new();
    }
}