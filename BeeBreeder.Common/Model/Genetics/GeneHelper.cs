using System;
using System.Collections.Generic;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Model.Genetics
{
    public static class GeneHelper
    {
        private static readonly Dictionary<Type, Func<object, IGene>> GeneGenerators = new();
        private static readonly Dictionary<Type, Func<IGene, IGene, IChromosome>> ChromosomeGenerators = new();

        static GeneHelper()
        {
            GeneGenerators.Add(typeof(int), GetIntGene);        
            GeneGenerators.Add(typeof(string), GetStringGene);
            GeneGenerators.Add(typeof(Adaptation), GetAdaptationGene);

            
            ChromosomeGenerators.Add(typeof(int), GetIntChromosome);
            ChromosomeGenerators.Add(typeof(string), GetStringChromosome);
            ChromosomeGenerators.Add(typeof(Adaptation), GetAdaptationChromosome);
        }
        
        public static IChromosome GetChromosome(string property, Type type, IGene primary, IGene secondary)
        {
            var chromosome = ChromosomeGenerators[type].Invoke(primary, secondary);
            chromosome.Property = property;
            return chromosome;
        }
        
        public static IGene GetGene(string property, object value, bool dominant)
        {
            var gene = GeneGenerators[value.GetType()].Invoke(value);
            gene.Dominant = dominant;
            return gene;
        }
        
        static IChromosome GetIntChromosome(IGene primary, IGene secondary)
        {
            return new Chromosome<int> {Primary = (Gene<int>)primary, Secondary = (Gene<int>)secondary};
        }
        
        static IChromosome GetStringChromosome(IGene primary, IGene secondary)
        {
            return new Chromosome<string> {Primary = (Gene<string>)primary, Secondary = (Gene<string>)secondary};
        }
        
        static IChromosome GetAdaptationChromosome(IGene primary, IGene secondary)
        {
            return new Chromosome<Adaptation> {Primary = (Gene<Adaptation>)primary, Secondary = (Gene<Adaptation>)secondary};
        }
        static IGene GetStringGene(object value)
        {
            return new Gene<string> {Value = (string)value};
        }

        static IGene GetIntGene(object value)
        {
            return new Gene<int> {Value = (int)value};
        }

        static IGene GetAdaptationGene(object value)
        {
            return new Gene<Adaptation>() {Value = (Adaptation)value};
        }
    }
}