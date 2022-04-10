using System;
using System.Text;

namespace BeeBreeder.Common.Model.Genetics
{
    [Serializable]
    public class Genotype
    {
        public ChromosomeDictionary Chromosomes { get; private set; } = new();
        
        public IChromosome this[string key]
        {
            get => Chromosomes[key];
            set => Chromosomes[key] = value;
        }
        
        public bool Equals(Genotype secondGenotype)
        {
            foreach (var gene in Chromosomes)
            {
                var secondGene = secondGenotype[gene.Key];
                var isEqual =
                    (gene.Value.Primary.Equals(secondGene.Primary) && gene.Value.Secondary.Equals(secondGene.Secondary)) ||
                    (gene.Value.Primary.Equals(secondGene.Secondary) && gene.Value.Secondary.Equals(secondGene.Primary));
                if (!isEqual)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var gene in Chromosomes)
            {
                sb.Append($"{gene.Key}: {gene.Value} \n");
            }

            return sb.ToString();
        }
    }
}