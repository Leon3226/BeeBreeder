using System;
using System.Collections.Generic;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth
{
    public static class GeneComparisonExtensions
    {
        private static readonly Dictionary<string, Func<int, int, Comparison>> IntComparers = new();
        private static readonly Dictionary<Type, Func<IGene, BreedingTarget, int>> Converters = new();

        static GeneComparisonExtensions()
        {
            IntComparers.Add(StatNames.Speed, MoreBetter);
            IntComparers.Add(StatNames.Fertility, MoreBetter);
            IntComparers.Add(StatNames.Lifespan, MoreBetter);
            IntComparers.Add(StatNames.Area, MoreBetter);
            IntComparers.Add(StatNames.Pollination, MoreBetter);
            IntComparers.Add(StatNames.Diurnal, MoreBetter);
            IntComparers.Add(StatNames.Nocturnal, MoreBetter);
            IntComparers.Add(StatNames.Cave, MoreBetter);
            IntComparers.Add(StatNames.Flyer, MoreBetter);
            IntComparers.Add(StatNames.Flowers, MoreBetter);
            IntComparers.Add(StatNames.Specie, MoreBetter);
            IntComparers.Add(StatNames.Effect, MoreBetter);
            IntComparers.Add(StatNames.HumidTolerance, MoreBetter);
            IntComparers.Add(StatNames.TempTolerance, MoreBetter);

            Converters.Add(typeof(Gene<Species>), ConvertSpecie);
            Converters.Add(typeof(Gene<Flowers>), ConvertFlowers);
            Converters.Add(typeof(Gene<Adaptation>), ConvertAdaptation);
            Converters.Add(typeof(Gene<Effect>), ConvertEffect);
        }

        private static Comparison MoreBetter(int gene1, int gene2)
        {
            if (gene1 == gene2)
                return Comparison.Equal;

            return gene1 > gene2 ? Comparison.Better : Comparison.Worse;
        }

        private static Comparison LessBetter(int gene1, int gene2)
        {
            if (gene1 == gene2)
                return Comparison.Equal;

            return gene1 < gene2 ? Comparison.Better : Comparison.Worse;
        }

        public static int Numeric(this IGene gene, BreedingTarget target = null)
        {
            var intValue = gene.Value as int?;
            if (intValue == null)
            {
                return Converters[gene.GetType()].Invoke(gene, target);
            }

            return intValue.Value;
        }

        public static Comparison Compare(this IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            var val1 = gene1.Numeric(target);
            var val2 = gene2.Numeric(target);
            return IntComparers[property].Invoke(val1, val2);
        }

        private static Comparison CompareInt(IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            return IntComparers[property].Invoke(((Gene<int>) gene1).Value, ((Gene<int>) gene2).Value);
        }

        private static int ConvertSpecie(IGene gene, BreedingTarget target = null)
        {
            var priorities = target == null || !target.PrioritizeSpecies
                ? BeeGeneticDatabase.DefaultSpeciePriorities
                : target.SpeciePriorities;
            //TODO: Add logic
            return priorities[((Gene<Species>) gene).Value];
        }

        private static int ConvertFlowers(IGene gene, BreedingTarget target = null)
        {
            //TODO: Add logic
            return BeeGeneticDatabase.DefaultFlowersPriorities[((Gene<Flowers>) gene).Value];
        }

        private static int ConvertEffect(IGene gene, BreedingTarget target = null)
        {
            //TODO: Add logic
            return BeeGeneticDatabase.DefaultEffectPriorities[((Gene<Effect>) gene).Value];
        }

        private static int ConvertAdaptation(IGene gene, BreedingTarget target = null)
        {
            var adp = (Gene<Adaptation>) gene;
            return adp.Value.Up + adp.Value.Down;
        }
    }
}