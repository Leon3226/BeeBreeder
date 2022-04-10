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
            GeneGenerators.Add(typeof(Species), GetSpecieGene);
            GeneGenerators.Add(typeof(Flowers), GetFlowerGene);           
            GeneGenerators.Add(typeof(Effect), GetEffectGene);
            GeneGenerators.Add(typeof(Adaptation), GetAdaptationGene);

            
            ChromosomeGenerators.Add(typeof(int), GetIntChromosome);
            ChromosomeGenerators.Add(typeof(Species), GetSpecieChromosome);
            ChromosomeGenerators.Add(typeof(Flowers), GetFlowerChromosome);
            ChromosomeGenerators.Add(typeof(Effect), GetEffectChromosome);
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
        static IChromosome GetFlowerChromosome(IGene primary, IGene secondary)
        {
            return new Chromosome<Flowers> {Primary = (Gene<Flowers>)primary, Secondary = (Gene<Flowers>)secondary};
        }
        static IChromosome GetSpecieChromosome(IGene primary, IGene secondary)
        {
            return new Chromosome<Species> { Primary = (Gene<Species>)primary, Secondary = (Gene<Species>)secondary};
        }
        
        static IChromosome GetEffectChromosome(IGene primary, IGene secondary)
        {
            return new Chromosome<Effect> {Primary = (Gene<Effect>)primary, Secondary = (Gene<Effect>)secondary};
        }
        
        static IChromosome GetAdaptationChromosome(IGene primary, IGene secondary)
        {
            return new Chromosome<Adaptation> {Primary = (Gene<Adaptation>)primary, Secondary = (Gene<Adaptation>)secondary};
        }
        static IGene GetFlowerGene(object value)
        {
            return new Gene<Flowers> {Value = (Flowers)value};
        }
        
        static IGene GetSpecieGene(object value)
        {
            return new Gene<Species> {Value = (Species)value};
        }
        
        static IGene GetEffectGene(object value)
        {
            return new Gene<Effect> {Value = (Effect)value};
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