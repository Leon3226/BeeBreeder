using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;

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
        public BeeCrossChance(Bee first, Bee second)
        {
            _first = first;
            _second = second;
            RecalculateChances();
        }

        private void RecalculateChances()
        {
            var firstSpecieChromosome = (SpecieChromosome)_first[BeeGeneticDatabase.StatNames.Specie];
            var secondSpecieChromosome = (SpecieChromosome)_second[BeeGeneticDatabase.StatNames.Specie];
            var mutationChances = SpecieChromosome.GetPossibleMutations(firstSpecieChromosome.Primary.Value,
                secondSpecieChromosome.Secondary.Value).Concat(SpecieChromosome.GetPossibleMutations(firstSpecieChromosome.Secondary.Value,
                secondSpecieChromosome.Primary.Value)).Distinct().ToList();

            var mutationGenomes = mutationChances.Select(x => (Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[x.MutationResult]), x.MutationChance)).ToArray();
            foreach (var firstGene in _first.Genotype.Chromosomes)
            {
                var secondGene = _second[firstGene.Key];
                IChromosomeCrossChance chance = ChromosomeCrossChanceHelper.GetChance(firstGene.Value, secondGene, BeeGeneticDatabase.StatTypes[secondGene.Property]
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