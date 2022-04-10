using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Model.Extensions
{
    public static class BeeStackExtensions
    {
        public static int OverallCount(this IEnumerable<BeeStack> stack)
        {
            return stack.Sum(x => x.Count);
        }

        public static Dictionary<Species, int> ExtractSpecies(this List<BeeStack> bees)
        {
            var species = bees.Select(x => x.Bee.SpecieChromosome.Primary.Value)
                .Concat(bees.Select(x => x.Bee.SpecieChromosome.Secondary.Value)).Distinct()
                .ToDictionary(x => x, x => 0);

            foreach (var bee in bees)
            {
                var specieChromosome = bee.Bee.SpecieChromosome;
                species[specieChromosome.Primary.Value] += bee.Count;
                species[specieChromosome.Secondary.Value] += bee.Count;
            }

            return species;
        }

        public static void RemoveCount(this List<BeeStack> bees, BeeStack beeStack, int count)
        {
            var stack = bees.FirstOrDefault(x => x == beeStack);
            if (stack != null)
            {
                stack.Count -= count;
                if (stack.Count <= 0)
                {
                    bees.Remove(beeStack);
                }
            }
            else
            {
                Console.WriteLine();
            }
        }

        public static BeeStack Copy(this BeeStack bees)
        {
            return new BeeStack(bees.Bee, bees.Count);
        }

        public static List<BeeStack> Copy(this IEnumerable<BeeStack> bees)
        {
            return bees.Select(x => x.Copy()).ToList();
        }
    }
}