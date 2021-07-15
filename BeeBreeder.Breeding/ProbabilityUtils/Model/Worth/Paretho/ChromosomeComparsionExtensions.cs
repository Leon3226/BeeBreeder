using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Paretho
{
    public static class ChromosomeComparsionExtensions
    {
        public static IChromosome ParethoBetter(this IChromosome first, IChromosome second, bool compareOrder = false, BreedingTarget target = null)
        {
            var fpp = first.Primary.Compare(second.Primary, first.Property);
            var fps = first.Primary.Compare(second.Secondary, first.Property);
            var fsp = first.Secondary.Compare(second.Primary, first.Property);
            var fss = first.Secondary.Compare(second.Secondary, first.Property);
            
            List<Comparison> comparisons = new List<Comparison>() {fpp, fps, fsp, fss};

            if (comparisons.Any(x => x == Comparison.Better) && comparisons.All(x => x != Comparison.Worse))
                return first;
            
            if (comparisons.Any(x => x == Comparison.Worse) && comparisons.All(x => x != Comparison.Better))
                return second;

            return null;
        }
    }
}