using BeeBreeder.Common.Data;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Generation
{
    public class BeeGenerator
    {
        private readonly ISpecieStatsRepository _specieStatsRepository;
        private readonly IGeneDominanceRepository _geneDominanceRepository;

        public BeeGenerator(ISpecieStatsRepository specieStatsRepository, IGeneDominanceRepository geneDominanceRepository)
        {
            _specieStatsRepository = specieStatsRepository;
            _geneDominanceRepository = geneDominanceRepository;
        }

        public Bee Generate(Species specie, Gender gender = Gender.Drone)
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
                var gene = GeneHelper.GetGene(stat.Key, stat.Value, _geneDominanceRepository.GenesDominance[stat.Key][stat.Value]);
                var chromosome = GeneHelper.GetChromosome(stat.Key, stat.Value.GetType(), gene, gene);
                newGenotype.Chromosomes.Add(stat.Key, chromosome);
            }

            return newGenotype;
        }
    }
}