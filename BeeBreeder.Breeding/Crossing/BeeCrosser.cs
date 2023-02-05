using System;
using System.Collections.Generic;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Crossing
{
    public class BeeCrosser : IBeeCrosser
    {
        private readonly ISpecieStatsProvider _specieStatsRepository;
        private readonly IGeneDominanceProvider _geneDominanceRepository;
        private readonly ISpecieCombinationsProvider _specieCombinationsRepository;
        private readonly BeeGenerator _beeGenerator;

        public BeeCrosser(ISpecieStatsProvider specieStatsRepository, IGeneDominanceProvider geneDominanceRepository, ISpecieCombinationsProvider specieCombinationsRepository, BeeGenerator beeGenerator)
        {
            _specieStatsRepository = specieStatsRepository;
            _geneDominanceRepository = geneDominanceRepository;
            _specieCombinationsRepository = specieCombinationsRepository;
            _beeGenerator = beeGenerator;
        }

        public List<Bee> Cross(Bee first, Bee second)
        {
            if (first.Gender == second.Gender)
                return null;

            List<Bee> child = new List<Bee>();
            var princess = first.Gender == Gender.Drone ? second : first;
            var fertility = ((IChromosome<int>)princess.Genotype[Constants.StatNames.Fertility]).ResultantAttribute;
            for (int i = 0; i < fertility + 1; i++)
            {
                child.Add(new Bee
                {
                    Gender = i == 0 ? Gender.Princess : Gender.Drone,
                    Genotype = CrossGenotypes(first.Genotype, second.Genotype),
                    Generation = princess.Generation + 1
                }
                );
            }

            return child;
        }

        public Genotype CrossGenotypes(Genotype first, Genotype second)
        {
            var newGenotype = new Genotype();
            if (first.Chromosomes.Count != second.Chromosomes.Count)
                throw new Exception("Genotypes doesnt match");

            var mutations = _specieCombinationsRepository.Mutations(
                (Chromosome<string>)first[Constants.StatNames.Specie],
                (Chromosome<string>)second[Constants.StatNames.Specie]);

            if (mutations.Item1 != null)
                first = _beeGenerator.GenotypeFromInitialStats(_specieStatsRepository.SpecieStats[mutations.Item1]);

            if (mutations.Item2 != null)
                second = _beeGenerator.GenotypeFromInitialStats(_specieStatsRepository.SpecieStats[mutations.Item2]);

            foreach (var gene in first.Chromosomes)
            {
                if (second.Chromosomes.TryGetValue(gene.Key, out var secondGene))
                    newGenotype.Chromosomes.Add(gene.Key, (IChromosome)gene.Value.Cross(secondGene));
                else
                    throw new Exception("Corresponded gene not found");
            }

            return newGenotype;
        }
    }
}
