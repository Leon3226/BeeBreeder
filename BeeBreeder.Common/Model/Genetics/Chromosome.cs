using System;
using System.Text.RegularExpressions;

namespace BeeBreeder.Common.Model.Genetics
{
    public class Chromosome<T> : IChromosome, IChromosome<T> where T : struct
    {
        public string Property { get; set; } = "Unnamed";
        public IGene<T> Primary { get; set; }
        public bool Clean => Primary.Value.Equals(Secondary.Value);

        public IGene<T> Secondary { get; set; }
        object IChromosome.ResultantAttribute => ResultantAttribute;

        public Chromosome()
        {
            
        }
        
        public Chromosome(IGene<T> value, string property = null)
        {
            if (property != null) Property = property;

            Primary = value;
            Secondary = value;
        }
        
        public Chromosome(T value, string property = null)
        {
            if (property != null) Property = property;
            var gene = new Gene<T>() { Value = value };

            Primary = gene;
            Secondary = gene;
        }

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

        public string StringResultantAttribute => ResultantAttribute.ToString();

        public IGene<T> Random()
        {
            return RandomGenerator.Double() > 0.5 ? Primary : Secondary;
        }

        protected virtual Chromosome<T> Cross(Chromosome<T> secondPair)
        {
            var newChromosome = GenesFromCrossing(secondPair);
            return new Chromosome<T>
            {
                Property = Property,
                Primary = newChromosome.Item1,
                Secondary = newChromosome.Item2
            };
        }

        protected (IGene<T>, IGene<T>) GenesFromCrossing(Chromosome<T> secondPair)
        {
            var first = Random();
            var second = secondPair.Random();
            var mixed = RandomGenerator.Double() > 0.5;
            if (!mixed)
                return (first, second);
            else
                return (second, first);
        }

        public override string ToString()
        {
            return $"{Primary} {Secondary} ({ResultantAttribute})";
        }

        public virtual ICrossable Cross(ICrossable second)
        {
            return Cross((Chromosome<T>)second);
        }

        IGene IChromosome.Primary => Primary;

        IGene IChromosome.Secondary => Secondary;
    }
}