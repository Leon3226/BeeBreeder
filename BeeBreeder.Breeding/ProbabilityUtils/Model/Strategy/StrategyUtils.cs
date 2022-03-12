using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy
{
    public class StrategyUtils : IStrategyUtils
    {
        public MutationTree Tree;

        public StrategyUtils(MutationTree tree)
        {
            Tree = tree;
        }

        public StrategyResult ImportantTargets(BeePool pool, int minimalCount = 5)
        {
            var result = new StrategyResult();
            var existingSpecies = pool.Bees
                .Select(x => x.Bee.SpecieChromosome.Primary.Value)
                .Concat(pool.Bees.Select(x => x.Bee.SpecieChromosome.Secondary.Value)
                ).Distinct().ToList();
            var possibleSpecies = Tree.PossibleResults(existingSpecies);

            var possibleSpeciesGenotypes = BeeGeneticDatabase.SpecieStats.Where(x => possibleSpecies.Contains(x.Key))
                .Select(x => (x.Key, Genotype.FromInitialStats(x.Value))).ToList();
            var referenceGenotype = possibleSpeciesGenotypes.FirstOrDefault().Item2;
            result.GenesBest = new();

            if (referenceGenotype != null)
            {
                foreach (var gene in referenceGenotype.Genes)
                {
                    var stat = gene.Key;
                    var geneBest = possibleSpeciesGenotypes
                        .Where(x => possibleSpeciesGenotypes.All(y =>
                            x.Item2[stat].ParetoBetter(y.Item2[stat]) != y.Item2[stat]))
                        .Select(x => (x.Key, x.Item2[stat])).ToList();
                    result.GenesBest.Add(stat, geneBest);
                }
            }

            foreach (var pair in result.GenesBest)
            {
                var bestGeneValue = pair.Value.FirstOrDefault().Item2.Primary;
                var bestGeneCount = pool.Bees.Count(x =>
                    x.Bee[pair.Key].Primary.Compare(bestGeneValue, pair.Key) != Comparison.Worse ||
                    x.Bee[pair.Key].Secondary.Compare(bestGeneValue, pair.Key) != Comparison.Worse);
                var enough = bestGeneCount >= minimalCount;
                if (enough)
                {
                    result.GenesBest.Remove(pair.Key);
                }
            }

            foreach (var pair in result.GenesBest)
            {
                result.GenesBest[pair.Key] = pair.Value.Where(val => !pair.Value.Any(x => Tree[x.Item1].LeadsTo(val.Item1)))
                    .ToList();
            }
            
            result.GenesBest.Values.ToList().ForEach(x => result.Species.AddRange(x.Select(y => y.Item1)));
            return result;
        }
    }
}