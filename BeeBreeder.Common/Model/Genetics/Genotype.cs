using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BeeBreeder.Common.AlleleDatabase.Bee;

namespace BeeBreeder.Common.Model.Genetics
{
    public class Genotype
    {
        private const string UnnamedGeneStringRegex = @"(\S+)\s+(\S+)";

        public readonly Dictionary<string, ICrossable> Genes = new();
        
        public ICrossable this[string key]
        {
            get => Genes[key];
            set => Genes[key] = value;
        }
        public Genotype Cross(Genotype second, Random random = null)
        {
            random ??= new Random();
            var newGenotype = new Genotype();
            if (Genes.Count != second.Genes.Count)
                throw new Exception("Genotypes doesnt match");

            var firstGenotype = this;
            var secondGenotype = second;
            
            var mutations = ((SpecieChromosome)Genes[BeeGeneticDatabase.StatNames.Specie]).Mutations((SpecieChromosome)second.Genes[BeeGeneticDatabase.StatNames.Specie], random);

            if (mutations.Item1 != null)
                firstGenotype = FromInitialStats(BeeGeneticDatabase.SpecieStats[mutations.Item1.Value]);
            
            if (mutations.Item2 != null)
                secondGenotype = FromInitialStats(BeeGeneticDatabase.SpecieStats[mutations.Item2.Value]);
            
            foreach (var gene in firstGenotype.Genes)
            {
                if (secondGenotype.Genes.TryGetValue(gene.Key, out var secondGene))
                    newGenotype.Genes.Add(gene.Key, gene.Value.Cross(secondGene, random));
                else
                    throw new Exception("Corresponded gene not found");
            }

            return newGenotype;
        }

        #region Genotype generation

        public static Genotype FromString(string text, GeneParsingMode mode = GeneParsingMode.Unnamed)
        {
            if (mode == GeneParsingMode.Unnamed)
                return FromStringUnnamed(text);

            return null;
        }
        public static Genotype FromInitialStats(BeeInitialStats stats)
        {
            var newGenotype = new Genotype();

            foreach (var stat in stats.Characteristics)
            {
                if (stat.Key == BeeGeneticDatabase.StatNames.Specie)
                {
                    var gene = new Gene<Species>
                        { Property = (Species) stats.Characteristics[stat.Key]};
                    gene.Dominant =
                        BeeGeneticDatabase.GenesDominancies[stat.Key][stat.Value];
                
                    newGenotype.Genes.Add(BeeGeneticDatabase.StatNames.Specie, new SpecieChromosome()
                    {
                        Property = stat.Key,
                        Primary = gene,
                        Secondary = gene,
                    });
                }
                else
                {
                    var gene = new Gene<object>
                        {Property = stats.Characteristics[stat.Key]};
                    gene.Dominant =
                        BeeGeneticDatabase.GenesDominancies[stat.Key][stat.Value];
                
                    newGenotype.Genes.Add(stat.Key, new Chromosome<object>
                    {
                        Property = stat.Key,
                        Primary = gene,
                        Secondary = gene,
                    });
                }
                
            }

            return newGenotype;
        }
        private static Genotype FromStringUnnamed(string text)
        {
            Regex rx = new Regex(UnnamedGeneStringRegex, RegexOptions.Compiled);
            MatchCollection matches = rx.Matches(text);
            var newGenotype = new Genotype();
            var i = 1;
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                newGenotype.Genes.Add(i.ToString(), new Chromosome<object>()
                {
                    Primary = new Gene<object> {Property = groups[1].Value},
                    Secondary = new Gene<object> {Property = groups[2].Value}
                } );
                i++;
            }

            return newGenotype;
        }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var gene in Genes)
            {
                sb.Append($"{gene.Key}: {gene.Value} \n");
            }

            return sb.ToString();
        }
    }
    
    
    public enum GeneParsingMode
    {
        Unnamed
    }
}