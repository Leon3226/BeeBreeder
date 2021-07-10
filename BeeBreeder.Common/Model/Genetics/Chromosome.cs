using System;
using System.Text.RegularExpressions;

namespace BeeBreeder.Common.Model.Genetics
{
    public class Chromosome<T> : ICrossable
    {
        public string Property = "Unnamed";
        public Gene<T> Primary;
        public Gene<T> Secondary;

        private Random _rand;

        public Gene<T> Random(Random random = null)
        {
            if (random == null)
                random = EnsureRandom();

            return random.NextDouble() > 0.5 ? Primary : Secondary;
        }

        private Random EnsureRandom()
        {
            _rand ??= new Random();
            return _rand;
        }

        protected virtual Chromosome<T> Cross(Chromosome<T> secondPair, Random random = null)
        {
            var newChromosome = GenesFromCrossing(secondPair, random);
            return new Chromosome<T>
            {
                Property = Property,
                Primary = newChromosome.Item1,
                Secondary = newChromosome.Item2
            };
        }

        protected (Gene<T>, Gene<T>) GenesFromCrossing(Chromosome<T> secondPair, Random random = null)
        {
            random ??= new Random();
            var first = Random(random);
            var second = secondPair.Random(random);
            var mixed = EnsureRandom().NextDouble() > 0.5;
            if (!mixed)
                return (first, second);
            else
                return (second, first);
        }

        public override string ToString()
        {
            return $"{Primary} {Secondary}";
        }

        public virtual ICrossable Cross(ICrossable second, Random random = null)
        {
            return Cross((Chromosome<T>)second, random);
        }
    }
}