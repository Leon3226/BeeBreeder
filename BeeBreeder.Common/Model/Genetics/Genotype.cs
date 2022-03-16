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

        public Dictionary<string, IChromosome> Genes { get; private set; } = new();
        
        public IChromosome this[string key]
        {
            get => Genes[key];
            set => Genes[key] = value;
        }
        public Genotype Cross(Genotype second)
        {
            var newGenotype = new Genotype();
            if (Genes.Count != second.Genes.Count)
                throw new Exception("Genotypes doesnt match");

            var firstGenotype = this;
            var secondGenotype = second;
            
            var mutations = ((SpecieChromosome)Genes[StatNames.Specie]).Mutations((SpecieChromosome)second[StatNames.Specie]);

            if (mutations.Item1 != null)
                firstGenotype = FromInitialStats(Db.SpecieStats[mutations.Item1.Value]);
            
            if (mutations.Item2 != null)
                secondGenotype = FromInitialStats(Db.SpecieStats[mutations.Item2.Value]);
            
            foreach (var gene in firstGenotype.Genes)
            {
                if (secondGenotype.Genes.TryGetValue(gene.Key, out var secondGene))
                    newGenotype.Genes.Add(gene.Key, (IChromosome)gene.Value.Cross(secondGene));
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
                newGenotype.Genes.Add(stat.Key, chromosome);
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
            foreach (var gene in Genes)
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