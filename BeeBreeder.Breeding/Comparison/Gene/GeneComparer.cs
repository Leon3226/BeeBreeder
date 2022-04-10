using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using BreederComparison = BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparison;

namespace BeeBreeder.Breeding.Comparison.Gene
{
    public class GeneComparer : IGeneComparer
    {
        private static readonly Dictionary<string, Func<int, int, BreederComparison>> IntComparers = new();
        private static readonly Dictionary<Type, Func<IGene, BreedingTarget, int>> Converters = new();

        public GeneComparer()
        {
            IntComparers.Add(Constants.StatNames.Speed, MoreBetter);
            IntComparers.Add(Constants.StatNames.Fertility, MoreBetter);
            IntComparers.Add(Constants.StatNames.Lifespan, LessBetter);
            IntComparers.Add(Constants.StatNames.Area, MoreBetter);
            IntComparers.Add(Constants.StatNames.Pollination, MoreBetter);
            IntComparers.Add(Constants.StatNames.Diurnal, MoreBetter);
            IntComparers.Add(Constants.StatNames.Nocturnal, MoreBetter);
            IntComparers.Add(Constants.StatNames.Cave, MoreBetter);
            IntComparers.Add(Constants.StatNames.Flyer, MoreBetter);
            IntComparers.Add(Constants.StatNames.Flowers, MoreBetter);
            IntComparers.Add(Constants.StatNames.Specie, MoreBetter);
            IntComparers.Add(Constants.StatNames.Effect, MoreBetter);
            IntComparers.Add(Constants.StatNames.HumidTolerance, MoreBetter);
            IntComparers.Add(Constants.StatNames.TempTolerance, MoreBetter);

            Converters.Add(typeof(Species), ConvertSpecie);
            Converters.Add(typeof(Flowers), ConvertFlowers);
            Converters.Add(typeof(Adaptation), ConvertAdaptation);
            Converters.Add(typeof(Effect), ConvertEffect);
        }

        private static BreederComparison MoreBetter(int gene1, int gene2)
        {
            if (gene1 == gene2)
                return BreederComparison.Equal;

            return gene1 > gene2 ? BreederComparison.Better : BreederComparison.Worse;
        }

        private static BreederComparison LessBetter(int gene1, int gene2)
        {
            if (gene1 == gene2)
                return BreederComparison.Equal;

            return gene1 < gene2 ? BreederComparison.Better : BreederComparison.Worse;
        }

        public int Numeric(IGene gene, BreedingTarget target = null)
        {
            var intValue = gene.Value as int?;
            if (intValue == null)
            {
                return Converters[gene.Type].Invoke(gene, target);
            }

            return intValue.Value;
        }

        public BreederComparison Compare(IGene gene1, IGene gene2, string property, BreedingTarget target = null)
        {
            var val1 = gene1.Numeric(target);
            var val2 = gene2.Numeric(target);
            return IntComparers[property].Invoke(val1, val2);
        }

        private static int ConvertSpecie(IGene gene, BreedingTarget target = null)
        {
            if (target == null || !target.PrioritizeSpecies || target.SpeciePriorities == null)
                return 0;

            if (target.SpeciePriorities.TryGetValue(((Gene<Species>)gene).Value, out int value))
            {
                return value;
            }

            return 0;
        }

        private static int ConvertFlowers(IGene gene, BreedingTarget target = null)
        {
            //TODO: Add logic
            return Constants.DefaultFlowersPriorities[((Gene<Flowers>)gene).Value];
        }

        private static int ConvertEffect(IGene gene, BreedingTarget target = null)
        {
            //TODO: Add logic
            return Constants.DefaultEffectPriorities[((Gene<Effect>)gene).Value];
        }

        private static int ConvertAdaptation(IGene gene, BreedingTarget target = null)
        {
            var adp = (Gene<Adaptation>)gene;
            return adp.Value.Up + adp.Value.Down;
        }
    }
}
