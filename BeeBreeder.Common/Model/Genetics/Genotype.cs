using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BeeBreeder.Common.AlleleDatabase.Bee;
using Newtonsoft.Json;
using Db =  BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Common.Model.Genetics
{
    [Serializable]
    public class Genotype
    {
        private const string UnnamedGeneStringRegex = @"(\S+)\s+(\S+)";

        public ChromosomeDictionary Chromosomes { get; private set; } = new();
        
        public IChromosome this[string key]
        {
            get => Chromosomes[key];
            set => Chromosomes[key] = value;
        }
        public Genotype Cross(Genotype second)
        {
            var newGenotype = new Genotype();
            if (Chromosomes.Count != second.Chromosomes.Count)
                throw new Exception("Genotypes doesnt match");

            var firstGenotype = this;
            var secondGenotype = second;
            
            var mutations = ((SpecieChromosome)Chromosomes[StatNames.Specie]).Mutations((SpecieChromosome)second[StatNames.Specie]);

            if (mutations.Item1 != null)
                firstGenotype = FromInitialStats(Db.SpecieStats[mutations.Item1.Value]);
            
            if (mutations.Item2 != null)
                secondGenotype = FromInitialStats(Db.SpecieStats[mutations.Item2.Value]);
            
            foreach (var gene in firstGenotype.Chromosomes)
            {
                if (secondGenotype.Chromosomes.TryGetValue(gene.Key, out var secondGene))
                    newGenotype.Chromosomes.Add(gene.Key, (IChromosome)gene.Value.Cross(secondGene));
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
                var gene = GeneHelper.GetGene(stat.Key, stat.Value, Db.GenesDominance[stat.Key][stat.Value]);
                var chromosome = GeneHelper.GetChromosome(stat.Key, stat.Value.GetType(), gene, gene);
                newGenotype.Chromosomes.Add(stat.Key, chromosome);
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
                //var groups = match.Groups;
                //newGenotype.Genes.Add(i.ToString(), new Chromosome<int>()
                //{
                //    Primary = new Gene<int> {Value = groups[1].Value},
                //    Secondary = new Gene<int> {Value = groups[2].Value}
                //} );
                //i++;
            }

            return newGenotype;
        }

        #endregion
        
        public bool Equals(Genotype secondGenotype)
        {
            // var secondGenotype = obj as Genotype;
            // if (secondGenotype == null)
            //     return false;
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
    
    
    public enum GeneParsingMode
    {
        Unnamed
    }
}