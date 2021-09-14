using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto
{
    public static class ChromosomeComparisonExtensions
    {
        public static IChromosome ParetoBetter(this IChromosome first, IChromosome second, bool compareOrder = false, BreedingTarget target = null)
        {
            var fpp = first.Primary.Compare(second.Primary, first.Property, target: target); 
            var fps = first.Primary.Compare(second.Secondary, first.Property, target: target);
            var fsp = first.Secondary.Compare(second.Primary, first.Property, target: target);
            var fss = first.Secondary.Compare(second.Secondary, first.Property, target: target);
            
            List<Comparison> comparisons = new List<Comparison>() {fpp, fps, fsp, fss};

            if (comparisons.Any(x => x == Comparison.Better))
            {
                if (comparisons.Any(x => x == Comparison.Worse))
                {
                    return null;
                }

                return first;
            }
            else
            {
                if (comparisons.All(x => x == Comparison.Equal))
                    return null;
            }

            return second;
        }
    }
}