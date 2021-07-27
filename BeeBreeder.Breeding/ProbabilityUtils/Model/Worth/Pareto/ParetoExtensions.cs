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
                var secondChromosome = second.Genotype.Genes[firstChromosome.Key];
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

        public static List<Bee> ParetoOptimal(this IList<Bee> bees)
        {
            List<Bee> optimal = bees.Where(bee => !bees.Any(x =>
            {
                var better = ParetoBetter(x, bee) == x;
                return better;
            })).ToList();
            return optimal;
        }
    }
}