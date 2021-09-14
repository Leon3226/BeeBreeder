using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;

namespace BeeBreeder.Common.Model.Genetics
{
    public class SpecieChromosome : Chromosome<Species>
    {
        public (Species?, Species?) Mutations(Chromosome<Species> secondChromosome)
        {
            Species? GetMutation(Species first, Species second)
            {
                return GetPossibleMutations(first, second).FirstOrDefault(x => x.MutationChance > RandomGenerator.Double())?.MutationResult;
            }

            Species? GetRandomMutation()
            {
                return RandomGenerator.Double() > 0.5
                    ? GetMutation(Primary.Value, secondChromosome.Secondary.Value)
                    : GetMutation(Secondary.Value, secondChromosome.Primary.Value);
            }
            
            return (GetRandomMutation(), GetRandomMutation());
        }

        protected override Chromosome<Species> Cross(Chromosome<Species> secondPair)
        {
            var newChromosome = GenesFromCrossing(secondPair);
            return new SpecieChromosome()
            {
                Property = Property,
                Primary = newChromosome.Item1,
                Secondary = newChromosome.Item2
            };
        }
        
        public static IEnumerable<SpecieCombination> GetPossibleMutations(Species first, Species second)
        {
            return BeeGeneticDatabase.SpecieCombinations.Where(x => 
                    (x.Parent1 == first && x.Parent2 == second) || 
                    (x.Parent2 == first && x.Parent1 == second));
        }
    }
}