using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Chance
{
    public class ChromosomeCrossChance<T> : IChromosomeCrossChance<T> where T : struct
    {
        private Chromosome<T> _first;
        public Chromosome<T> First
        {
            get => _first;
            set
            {
                _first = value;
                RecalculateChances();
            }
        }

        IChromosome IChromosomeCrossChance.Second => _second;

        private Chromosome<T> _second;
        IChromosome IChromosomeCrossChance.First => _first;

        public Chromosome<T> Second
        {
            get => _second;
            set
            {
                _second = value;
                RecalculateChances();
            }
        }

        private List<(Chromosome<T>, double)> _mutations = new();

        private double SumChance
        {
            get
            {
                return _chances.Sum(x => x.Probability);
            }
        }

        public IReadOnlyCollection<(Chromosome<T>, double)> Mutations => _mutations.AsReadOnly();

        public ChromosomeCrossChance(Chromosome<T> first, Chromosome<T> second,
            params (Chromosome<T>, double)[] mutationChances)
        {
            _first = first;
            _second = second;
            _mutations = mutationChances.ToList();
            RecalculateChances();
        }

        public void ClearMutations()
        {
            _mutations.Clear();
            RecalculateChances();
        }

        IReadOnlyCollection<Chance<IChromosome>> IChromosomeCrossChance.Chances => Chances.Select(x => new Chance<IChromosome>() {Probability = x.Probability, Value = x.Value}).ToList().AsReadOnly();

        public void WithMutationChances(params (Chromosome<T>, double)[] mutationChances)
        {
            _mutations = mutationChances.ToList();
            RecalculateChances();
        }

        private readonly List<Chance<Chromosome<T>>> _chances = new();

        public IReadOnlyCollection<Chance<Chromosome<T>>> Chances => _chances.AsReadOnly();

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var chance in _chances)
            {
                sb.Append($"{chance} \n");
            }

            return sb.ToString();
        }

        private void RecalculateChances()
        {
            if (First == null || Second == null || First.Property != Second.Property)
                return;

            var geneProperty = First.Property;

            _chances.Clear();
            var leftSide = _mutations.ToList();
            var rightSide = _mutations.ToList();
            leftSide.Add((_first, 1));
            rightSide.Add((_second, 1));
            
            double sumPercent = 0;
            double resultPercent = 1;
            
            foreach (var first in leftSide)
            {
                foreach (var second in rightSide)
                {
                    if (first.Item1 == _first && second.Item1 == _second) 
                        continue;
                    
                    var mutationChance = first.Item2 * second.Item2;
                    sumPercent += mutationChance;
                    resultPercent -= resultPercent * mutationChance;
                }
            }

            var resultMutationChance = 1 - resultPercent;

            foreach (var first in leftSide)
            {
                foreach (var second in rightSide)
                {
                    double chance;
                    if (first.Item1 == _first && second.Item1 == _second)
                    {
                        chance = resultPercent;
                    }
                    else
                    {
                        var mutationChance = first.Item2 * second.Item2;
                        chance = (mutationChance / sumPercent) * resultMutationChance;
                    }
                    var chances = GetChances(first.Item1, second.Item1, chance);
                    _chances.AddRange(chances);
                }
            }

            CutSameChances();


            Chance<Chromosome<T>> GetChance(IGene<T> primary, IGene<T> secondary, string property, double probability)
            {
                return new()
                {
                    Probability = probability,
                    Value = new Chromosome<T>()
                    {
                        Property = property,
                        Primary = primary,
                        Secondary = secondary
                    }
                };
            }

            List<Chance<Chromosome<T>>> GetChances(Chromosome<T> firstChromosome, Chromosome<T> secondChromosome,
                double probability)
            {
                List<Chance<Chromosome<T>>> newChances = new();
                var prob = probability * 0.25;
                newChances.Add(GetChance(firstChromosome.Primary, secondChromosome.Primary, geneProperty, prob));
                newChances.Add(GetChance(firstChromosome.Primary, secondChromosome.Secondary, geneProperty, prob));
                newChances.Add(GetChance(firstChromosome.Secondary, secondChromosome.Primary, geneProperty, prob));
                newChances.Add(GetChance(firstChromosome.Secondary, secondChromosome.Secondary, geneProperty, prob));
                return newChances;
            }

            void CutSameChances()
            {
                for (int i = 0; i < _chances.Count; i++)
                {
                    var original = _chances[i];
                    var sameValues = _chances.Where(x =>
                        x != original &&
                        ((x.Value.Primary.Value.Equals(original.Value.Primary.Value) &&
                          x.Value.Secondary.Value.Equals(original.Value.Secondary.Value)) ||
                         (x.Value.Secondary.Value.Equals(original.Value.Primary.Value) &&
                          x.Value.Primary.Value.Equals(original.Value.Secondary.Value)))
                    ).ToArray();

                    foreach (var item in sameValues)
                    {
                        original.Probability += item.Probability;
                        _chances.Remove(item);
                    }
                }
            }
        }
    }
}