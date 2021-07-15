using System.Collections.Generic;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Common.Model.Bees
{
    public class Bee
    {
        public Gender Gender;
        public Genotype Genotype;

        public List<Bee> Breed(Bee secondBee)
        {
            if (Gender == secondBee.Gender)
                return null;

            List<Bee> child = new List<Bee>();
            var princess = Gender == Gender.Drone ? secondBee : this;
            var fertility = ((IChromosome<int>)princess.Genotype[BeeGeneticDatabase.StatNames.Fertility]).ResultantAttribute;
            for (int i = 0; i < fertility + 1; i++)
            {
                child.Add(new Bee()
                    {
                        Gender = i == 0 ? Gender.Princess : Gender.Drone,
                        Genotype = Genotype.Cross(secondBee.Genotype)
                    }
                );
            }

            return child;
        }

        public override string ToString()
        {
            return $"{Gender} \n{Genotype}";
        }
    }
}