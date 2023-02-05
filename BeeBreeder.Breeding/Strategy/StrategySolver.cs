using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.Comparison.Gene.Comparators;
using BeeBreeder.Breeding.Comparison.Pareto;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Extensions;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Strategy
{
    //TODO: Completely refactor code mess
    public class StrategySolver : IStrategySolver
    {
        private readonly MutationTree _mutationTree;
        private readonly ISpecieStatsProvider _specieStatsRepository;
        private readonly BeeGenerator _beeGenerator;
        private readonly IParetoComparer _paretoComparer;
        private readonly IGeneComparator _geneComparator;


        public StrategySolver(MutationTree tree, ISpecieStatsProvider specieStatsRepository, BeeGenerator beeGenerator, IParetoComparer paretoComparer, IGeneComparator geneComparator)
        {
            _mutationTree = tree;
            _specieStatsRepository = specieStatsRepository;
            _beeGenerator = beeGenerator;
            _paretoComparer = paretoComparer;
            _geneComparator = geneComparator;
        }

        public StrategyResult ImportantTargets(BeePool pool, int minimalCount = 5)
        {
            var result = new StrategyResult();
            var existingSpecies = pool.Bees.ExtractSpecies().Select(x => x.Key).ToList();
            var possibleSpecies = _mutationTree.PossibleResults(existingSpecies);

            var possibleSpeciesGenotypes = _specieStatsRepository.SpecieStats.Where(x => possibleSpecies.Contains(x.Key))
                .Select(x => (x.Key, _beeGenerator.GenotypeFromInitialStats(x.Value))).ToList();
            var referenceGenotype = possibleSpeciesGenotypes.FirstOrDefault().Item2;
            result.GenesBest = BestGenes(possibleSpeciesGenotypes);

            foreach (var pair in result.GenesBest)
            {
                if (!pair.Value.Any())
                {
                    continue;
                }
                var bestGeneValue = pair.Value.FirstOrDefault().Item2.Primary;
                var bestGeneCount = pool.Bees.Count(x =>
                    _geneComparator.Compare(x.Bee[pair.Key].Primary.Value, bestGeneValue.Value, pair.Key) != Comparison.Gene.Comparison.Worse ||
                    _geneComparator.Compare(x.Bee[pair.Key].Secondary.Value, bestGeneValue.Value, pair.Key) != Comparison.Gene.Comparison.Worse);
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

        private Dictionary<string, List<(string, IChromosome)>> BestGenes(IEnumerable<(string key, Genotype genotype)> possibleSpeciesGenotypes)
        {
            var result = new Dictionary<string, List<(string, IChromosome)>>();
            var referenceGenotype = possibleSpeciesGenotypes.FirstOrDefault().Item2;

            if (referenceGenotype != null)
            {
                foreach (var gene in referenceGenotype.Chromosomes)
                {
                    var stat = gene.Key;
                    var bestGenes = new List<(string, IChromosome)>();
                    var allStatGenesToCheck = possibleSpeciesGenotypes.Select(x => (x.key, x.Item2.Chromosomes[stat])).ToList();
                    var removedGenes = new HashSet<(string key, IChromosome)>();

                    for (int i = 0; i < allStatGenesToCheck.Count; i++)
                    {
                        var first = allStatGenesToCheck[i];
                        if (removedGenes.Contains(first))
                            continue;
                        for (int j = i + 1; j < allStatGenesToCheck.Count; j++)
                        {
                            var second = allStatGenesToCheck[j];
                            if (removedGenes.Contains(second))
                                continue;
                            var better = _paretoComparer.ParetoBetter(first.Item2, second.Item2);
                            var worse = better == first.Item2 ? second : first;
                            removedGenes.Add(worse);
                            if (worse == first)
                                break;
                        }
                    }

                    var genesBest = allStatGenesToCheck.Except(removedGenes).Select(x => (x.key, x.Item2)).ToList();
                    result.Add(stat, genesBest);
                }
            }

            return result;
        }
    }
}