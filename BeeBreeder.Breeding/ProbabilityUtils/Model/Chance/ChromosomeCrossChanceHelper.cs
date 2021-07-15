using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Chance
{
    public static class ChromosomeCrossChanceHelper
    {
        private static readonly
            Dictionary<Type, Func<IChromosome, IChromosome, (IChromosome, double)[], IChromosomeCrossChance>>
            ChanceGenerators = new();

        static ChromosomeCrossChanceHelper()
        {
            ChanceGenerators.Add(typeof(int), GetIntChance);
            ChanceGenerators.Add(typeof(Species), GetSpecieChance);
            ChanceGenerators.Add(typeof(Flowers), GetFlowerChance);           
            ChanceGenerators.Add(typeof(Effect), GetEffectChance);
            ChanceGenerators.Add(typeof(Adaptation), GetAdaptationChance);
        }

        public static IChromosomeCrossChance GetChance(IChromosome primary, IChromosome secondary, Type type,
            params (IChromosome, double)[] mutations)
        {
            var chance = ChanceGenerators[type].Invoke(primary, secondary, mutations);
            return chance;
        }

        static IChromosomeCrossChance GetIntChance(IChromosome primary, IChromosome secondary,
            params (IChromosome, double)[] mutations)
        {
            return new ChromosomeCrossChance<int>((Chromosome<int>) primary, (Chromosome<int>) secondary,
                mutations.Select(x => ((Chromosome<int>) x.Item1, x.Item2)).ToArray());
        }
        
        static IChromosomeCrossChance GetSpecieChance(IChromosome primary, IChromosome secondary,
            params (IChromosome, double)[] mutations)
        {
            return new ChromosomeCrossChance<Species>((Chromosome<Species>) primary, (Chromosome<Species>) secondary,
                mutations.Select(x => ((Chromosome<Species>) x.Item1, x.Item2)).ToArray());
        }
        
        static IChromosomeCrossChance GetFlowerChance(IChromosome primary, IChromosome secondary,
            params (IChromosome, double)[] mutations)
        {
            return new ChromosomeCrossChance<Flowers>((Chromosome<Flowers>) primary, (Chromosome<Flowers>) secondary,
                mutations.Select(x => ((Chromosome<Flowers>) x.Item1, x.Item2)).ToArray());
        }
        
        static IChromosomeCrossChance GetEffectChance(IChromosome primary, IChromosome secondary,
            params (IChromosome, double)[] mutations)
        {
            return new ChromosomeCrossChance<Effect>((Chromosome<Effect>) primary, (Chromosome<Effect>) secondary,
                mutations.Select(x => ((Chromosome<Effect>) x.Item1, x.Item2)).ToArray());
        }
        
        static IChromosomeCrossChance GetAdaptationChance(IChromosome primary, IChromosome secondary,
            params (IChromosome, double)[] mutations)
        {
            return new ChromosomeCrossChance<Adaptation>((Chromosome<Adaptation>) primary, (Chromosome<Adaptation>) secondary,
                mutations.Select(x => ((Chromosome<Adaptation>) x.Item1, x.Item2)).ToArray());
        }
    }
}