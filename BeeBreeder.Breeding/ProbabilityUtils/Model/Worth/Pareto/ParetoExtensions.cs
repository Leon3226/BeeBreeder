using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto
{
    public static class ParetoExtensions
    {
        public static Bee ParetoBetter(this Bee first, Bee second)
        {
            bool firstHasBest = false;
            bool secondHasBest = false;

            foreach (var firstChromosome in first.Genotype.Genes)
            {
                var secondChromosome = second[firstChromosome.Key];
                var best = firstChromosome.Value.ParetoBetter(secondChromosome);
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

        public static List<Bee> ParetoOptimal(this IEnumerable<Bee> bees)
        {
            var toCheck = bees.ToList();
            for (int i = 0; i < toCheck.Count; i++)
            {
                var bee1 = toCheck[i];
                for (int j = 0; j < toCheck.Count; j++)
                {
                    var bee2 = toCheck[j];
                    if (bee1 == bee2) continue;
                    var better = ParetoBetter(bee1, bee2);
                    if (better != null)
                    {
                        var worse = better == bee1 ? bee2 : bee1;
                        toCheck.Remove(worse);
                        if (better == bee1) break;
                    }
                }
            }
            return toCheck;
        }
        
        public static List<BeeStack> ParetoOptimal(this IEnumerable<BeeStack> bees)
        {
            var optimal = ParetoOptimal(bees.Select(x => x.Bee));
            return bees.Where(x => optimal.Contains(x.Bee)).ToList();
        }
    }
}