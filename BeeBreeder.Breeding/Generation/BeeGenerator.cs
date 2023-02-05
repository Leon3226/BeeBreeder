using BeeBreeder.Common.Data;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Generation
{
    public class BeeGenerator
    {
        private readonly ISpecieStatsProvider _specieStatsRepository;
        private readonly IGeneDominanceProvider _geneDominanceRepository;

        public BeeGenerator(ISpecieStatsProvider specieStatsRepository, IGeneDominanceProvider geneDominanceRepository)
        {
            _specieStatsRepository = specieStatsRepository;
            _geneDominanceRepository = geneDominanceRepository;
        }

        public Bee Generate(string specie, Gender gender = Gender.Drone)
        {
            return new()
            {
                Gender = gender,
                Genotype = GenotypeFromInitialStats(_specieStatsRepository.SpecieStats[specie])
            };
        }

        public Genotype GenotypeFromInitialStats(BeeInitialStats stats)
        {
            var newGenotype = new Genotype();

            foreach (var stat in stats.Characteristics)
            {
                var foundDominance = _geneDominanceRepository.GenesDominance[stat.Key].TryGetValue(stat.Value, out bool dominant);
                var gene = GeneHelper.GetGene(stat.Key, stat.Value, foundDominance ? dominant : false);
                var chromosome = GeneHelper.GetChromosome(stat.Key, stat.Value.GetType(), gene, gene);
                newGenotype.Chromosomes.Add(stat.Key, chromosome);
            }

            return newGenotype;
        }
    }
}