using System.Linq;
using BeeBreeder.Breeding.Comparison.Pareto;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy
{
    public class StrategyUtils : IStrategyUtils
    {
        private readonly MutationTree _mutationTree;
        private readonly ISpecieStatsRepository _specieStatsRepository;
        private readonly BeeGenerator _beeGenerator;
        private readonly IParetoComparer _paretoComparer;
        

        public StrategyUtils(MutationTree tree, ISpecieStatsRepository specieStatsRepository, BeeGenerator beeGenerator, IParetoComparer paretoComparer)
        {
            _mutationTree = tree;
            _specieStatsRepository = specieStatsRepository;
            _beeGenerator = beeGenerator;
            _paretoComparer = paretoComparer;
        }

        public StrategyResult ImportantTargets(BeePool pool, int minimalCount = 5)
        {
            var result = new StrategyResult();
            var existingSpecies = pool.Bees
                .Select(x => x.Bee.SpecieChromosome.Primary.Value)
                .Concat(pool.Bees.Select(x => x.Bee.SpecieChromosome.Secondary.Value)
                ).Distinct().ToList();
            var possibleSpecies = _mutationTree.PossibleResults(existingSpecies);

            var possibleSpeciesGenotypes = _specieStatsRepository.SpecieStats.Where(x => possibleSpecies.Contains(x.Key))
                .Select(x => (x.Key, _beeGenerator.GenotypeFromInitialStats(x.Value))).ToList();
            var referenceGenotype = possibleSpeciesGenotypes.FirstOrDefault().Item2;
            result.GenesBest = new();

            if (referenceGenotype != null)
            {
                foreach (var gene in referenceGenotype.Chromosomes)
                {
                    var stat = gene.Key;
                    var geneBest = possibleSpeciesGenotypes
                        .Where(x => possibleSpeciesGenotypes.All(y =>
                            _paretoComparer.ParetoBetter(x.Item2[stat], y.Item2[stat]) != y.Item2[stat]))
                        .Select(x => (x.Key, x.Item2[stat])).ToList();
                    result.GenesBest.Add(stat, geneBest);
                }
            }

            foreach (var pair in result.GenesBest)
            {
                var bestGeneValue = pair.Value.FirstOrDefault().Item2.Primary;
                var bestGeneCount = pool.Bees.Count(x =>
                    x.Bee[pair.Key].Primary.Compare(bestGeneValue, pair.Key) != Worth.Comparison.Worse ||
                    x.Bee[pair.Key].Secondary.Compare(bestGeneValue, pair.Key) != Worth.Comparison.Worse);
                var enough = bestGeneCount >= minimalCount;
                if (enough)
                {
                    result.GenesBest.Remove(pair.Key);
                }
            }

            foreach (var pair in result.GenesBest)
            {
                result.GenesBest[pair.Key] = pair.Value.Where(val => !pair.Value.Any(x => _mutationTree[x.Item1].LeadsTo(val.Item1)))
                    .ToList();
            }
            
            result.GenesBest.Values.ToList().ForEach(x => result.Species.AddRange(x.Select(y => y.Item1)));
            return result;
        }
    }
}