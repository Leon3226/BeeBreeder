using System;
using System.Collections.Generic;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth
{
    public static class GeneComparisonExtensions
    {
        private static readonly Dictionary<string, Func<int, int, Comparison>> IntComparers = new ();
        private static readonly Dictionary<Type, Func<IGene, IGene, string, BreedingTarget, Comparison>> Comparers = new ();

        static GeneComparisonExtensions()
        {
            IntComparers.Add(BeeGeneticDatabase.StatNames.Speed, MoreBetter);
            IntComparers.Add(BeeGeneticDatabase.StatNames.Fertility, MoreBetter);
            IntComparers.Add(BeeGeneticDatabase.StatNames.Lifespan, MoreBetter);
            IntComparers.Add(BeeGeneticDatabase.StatNames.Area, MoreBetter);
            IntComparers.Add(BeeGeneticDatabase.StatNames.Pollination, MoreBetter);
            IntComparers.Add(BeeGeneticDatabase.StatNames.Diurnal, MoreBetter);
            IntComparers.Add(BeeGeneticDatabase.StatNames.Nocturnal, MoreBetter);
            IntComparers.Add(BeeGeneticDatabase.StatNames.Cave, MoreBetter);
            IntComparers.Add(BeeGeneticDatabase.StatNames.Flyer, MoreBetter);
            
            Comparers.Add(typeof(Gene<int>), CompareInt);
            Comparers.Add(typeof(Gene<Species>), CompareSpecie);
            Comparers.Add(typeof(Gene<Flowers>), CompareFlowers);
            Comparers.Add(typeof(Gene<Adaptation>), CompareAdaptation);
            Comparers.Add(typeof(Gene<Effect>), CompareEffect);

        }

        private static Comparison MoreBetter(int gene1, int gene2)
        {
            if (gene1 == gene2)
                return Comparison.Equal;

            return gene1 > gene2 ? Comparison.Better : Comparison.Worse;
        }
        
        public static Comparison LessBetter(int gene1, int gene2)
        {
            if (gene1 == gene2)
                return Comparison.Equal;

            return gene1 < gene2 ? Comparison.Better : Comparison.Worse;
        }
        
        public static Comparison Compare(this IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            return Comparers[gene1.GetType()].Invoke(gene1, gene2, property, target);
        }
        private static Comparison CompareInt(IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            return IntComparers[property].Invoke(((Gene<int>)gene1).Value, ((Gene<int>)gene2).Value);
        }
        private static Comparison CompareSpecie(IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            //TODO: Add logic
            var adp1 = (Gene<Species>)gene1;
            var adp2 = (Gene<Species>)gene2;
            var val1 = BeeGeneticDatabase.DefaultSpeciePriorities[adp1.Value];
            var val2 = BeeGeneticDatabase.DefaultSpeciePriorities[adp2.Value];
            if (val1 == val2)
                return Comparison.Equal;

            return val1 > val2 ? Comparison.Better : Comparison.Worse;
        }
        private static Comparison CompareFlowers(IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            //TODO: Add logic
            var adp1 = (Gene<Flowers>)gene1;
            var adp2 = (Gene<Flowers>)gene2;
            
            var val1 = BeeGeneticDatabase.DefaultFlowersPriorities[adp1.Value];
            var val2 = BeeGeneticDatabase.DefaultFlowersPriorities[adp2.Value];
            if (val1 == val2)
                return Comparison.Equal;

            return val1 > val2 ? Comparison.Better : Comparison.Worse;
        }
        
        private static Comparison CompareEffect(IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            //TODO: Add logic
            var adp1 = (Gene<Effect>)gene1;
            var adp2 = (Gene<Effect>)gene2;
            
            var val1 = BeeGeneticDatabase.DefaultEffectPriorities[adp1.Value];
            var val2 = BeeGeneticDatabase.DefaultEffectPriorities[adp2.Value];
            if (val1 == val2)
                return Comparison.Equal;

            return val1 > val2 ? Comparison.Better : Comparison.Worse;
        }

        private static Comparison CompareAdaptation(IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            var adp1 = (Gene<Adaptation>)gene1;
            var adp2 = (Gene<Adaptation>)gene2;
            var val1 = adp1.Value.Up + adp1.Value.Down;
            var val2 = adp2.Value.Up + adp2.Value.Down;
            if (val1 == val2)
                return Comparison.Equal;

            return val1 > val2 ? Comparison.Better : Comparison.Worse;
        }
        
    }
}