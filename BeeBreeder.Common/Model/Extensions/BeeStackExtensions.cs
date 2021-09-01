using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;

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
    }
}