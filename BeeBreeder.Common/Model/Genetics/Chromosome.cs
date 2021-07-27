using System;
using System.Text.RegularExpressions;

namespace BeeBreeder.Common.Model.Genetics
{
    public class Chromosome<T> : IChromosome, IChromosome<T> where T : struct
    {
        public string Property { get; set; } = "Unnamed";
        public IGene<T> Primary { get; set; }
        public IGene<T> Secondary { get; set; }

        public T ResultantAttribute
        {
            get
            {
                if (Primary.Dominant && !Secondary.Dominant)
                    return Primary.Value;
                if (!Primary.Dominant && Secondary.Dominant)
                    return Secondary.Value;

                return Primary.Value;
            }
        }
        public IGene<T> Random(Random random = null)
        {
            random ??= new Random();

            return random.NextDouble() > 0.5 ? Primary : Secondary;
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

        protected (IGene<T>, IGene<T>) GenesFromCrossing(Chromosome<T> secondPair, Random random = null)
        {
            random ??= new Random();
            var first = Random(random);
            var second = secondPair.Random(random);
            var mixed = random.NextDouble() > 0.5;
            if (!mixed)
                return (first, second);
            else
                return (second, first);
        }

        public override string ToString()
        {
            return $"{Primary} {Secondary} ({ResultantAttribute})";
        }

        public virtual ICrossable Cross(ICrossable second, Random random = null)
        {
            return Cross((Chromosome<T>)second, random);
        }

        IGene IChromosome.Primary => Primary;

        IGene IChromosome.Secondary => Secondary;
    }
}