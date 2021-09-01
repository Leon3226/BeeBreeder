using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto
{
    public static class ParetoExtensions
    {
        public static Bee ParetoBetter(this Bee first, Bee second, BreedingTarget target = null)
        {
            bool firstHasBest = false;
            bool secondHasBest = false;

            foreach (var firstChromosome in first.Genotype.Genes)
            {
                var secondChromosome = second[firstChromosome.Key];
                var best = firstChromosome.Value.ParetoBetter(secondChromosome, target: target);
                if (best == firstChromosome.Value)
                    firstHasBest = true;
                if (best == secondChromosome)
                    secondHasBest = true;
                if (firstHasBest && secondHasBest)
                    return null;
            }

            if (firstHasBest)
                return first;

            if (secondHasBest)
                return second;

            return null;
        }

        public static List<Bee> ParetoOptimal(this IEnumerable<Bee> bees, BreedingTarget target = null)
        {
            var toCheck = bees.ToList();
            for (int i = 0; i < toCheck.Count; i++)
            {
                var bee1 = toCheck[i];
                for (int j = 0; j < toCheck.Count; j++)
                {
                    var bee2 = toCheck[j];
                    if (bee1 == bee2) continue;
                    var better = ParetoBetter(bee1, bee2, target);
                    if (better != null)
                    {
                        var worse = better == bee1 ? bee2 : bee1;
                        toCheck.Remove(worse);
                        if (worse == bee1)
                        {
                            i--;
                            break;
                        }
                        else j--;
                    }
                }
            }

            return toCheck;
        }

        public static List<BeeStack> ParetoOptimal(this IEnumerable<BeeStack> bees, BreedingTarget target = null)
        {
            var optimal = ParetoOptimal(bees.Select(x => x.Bee), target);
            return bees.Where(x => optimal.Contains(x.Bee)).ToList();
        }
    }
}