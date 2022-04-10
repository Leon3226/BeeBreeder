using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using StatNames = BeeBreeder.Common.Data.Constants.StatNames;

namespace BeeBreeder.Common.Model.Bees
{
    [Serializable]
    public class Bee
    {
        public Gender Gender { get; set; }
        public Genotype Genotype { get; set; } = new();
        public int Generation { get; set; }

        public IChromosome this[string index]
        {
            get => Genotype[index];
            set => Genotype[index] = value;
        }

        public Chromosome<Species> SpecieChromosome => ChromosomeOf<Species>(StatNames.Specie);

        public Chromosome<T> ChromosomeOf<T>(string property = null) where T : struct
        {
            return Genotype.Chromosomes.FirstOrDefault(x =>
                    (property == null || property == x.Key) && x.Value.Primary.Value.GetType() == typeof(T))
                .Value as Chromosome<T>;
        }

        public bool Equals(Bee secondBee)
        {
            return Gender == secondBee.Gender && Genotype.Equals(secondBee.Genotype);
        }

        public override string ToString()
        {
            return $"{Gender} \n{Genotype}";
        }
    }
}