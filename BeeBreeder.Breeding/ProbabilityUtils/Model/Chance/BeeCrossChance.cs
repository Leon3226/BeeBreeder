using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Chance
{
    public class BeeCrossChance
    {
        private Bee _first;
        public Bee First
        {
            get => _first;
            set
            {
                _first = value;
                RecalculateChances();
            }
        }

        private Bee _second;
        public Bee Second
        {
            get => _second;
            set
            {
                _second = value;
                RecalculateChances();
            }
        }
        public readonly List<IChromosomeCrossChance> Chances = new();

        private readonly ISpecieCombinationsRepository _specieCombinationsRepository;
        private readonly BeeGenerator _beeGenerator;
        private readonly ISpecieStatsRepository _specieStatsRepository;

        public BeeCrossChance(ISpecieCombinationsRepository specieCombinationsRepository, BeeGenerator beeGenerator, ISpecieStatsRepository specieStatsRepository)
        {
            _specieCombinationsRepository = specieCombinationsRepository;
            _beeGenerator = beeGenerator;
            _specieStatsRepository = specieStatsRepository;
        }

        public BeeCrossChance(Bee first, Bee second)
        {
            _first = first;
            _second = second;
            RecalculateChances();
        }

        private void RecalculateChances()
        {
            var firstSpecieChromosome = (Chromosome<Species>)_first[Constants.StatNames.Specie];
            var secondSpecieChromosome = (Chromosome<Species>)_second[Constants.StatNames.Specie];
            var mutationChances = _specieCombinationsRepository.GetPossibleMutations(firstSpecieChromosome.Primary.Value,
                secondSpecieChromosome.Secondary.Value).Concat(_specieCombinationsRepository.GetPossibleMutations(firstSpecieChromosome.Secondary.Value,
                secondSpecieChromosome.Primary.Value)).Distinct().ToList();

            var mutationGenomes = mutationChances.Select(x => (_beeGenerator.GenotypeFromInitialStats(_specieStatsRepository.SpecieStats[x.MutationResult]), x.MutationChance)).ToArray();
            foreach (var firstGene in _first.Genotype.Chromosomes)
            {
                var secondGene = _second[firstGene.Key];
                IChromosomeCrossChance chance = ChromosomeCrossChanceHelper.GetChance(firstGene.Value, secondGene, Constants.StatTypes[secondGene.Property]
                    , mutationGenomes.Select(x => (x.Item1[firstGene.Key], x.MutationChance)).ToArray());
                Chances.Add(chance);
            }
        }
        
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Chances.Count);
            sb.Append(": \n");
            foreach (var chance in Chances)
            {
                sb.Append($"{chance.First.Property}: \n");
                sb.Append(chance);
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}