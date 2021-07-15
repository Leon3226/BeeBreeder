using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;

namespace BeeBreeder.Common.Model.Genetics
{
    public class SpecieChromosome : Chromosome<Species>
    {
        public (Species?, Species?) Mutations(Chromosome<Species> secondChromosome, Random random = null)
        {
            random ??= new Random();

            Species? GetMutation(Species first, Species second)
            {
                return GetPossibleMutations(first, second,random).FirstOrDefault(x => x.MutationChance > random.NextDouble())?.MutationResult;
            }

            Species? GetRandomMutation()
            {
                return random.NextDouble() > 0.5
                    ? GetMutation(Primary.Value, secondChromosome.Secondary.Value)
                    : GetMutation(Secondary.Value, secondChromosome.Primary.Value);
            }
            
            return (GetRandomMutation(), GetRandomMutation());
        }

        protected override Chromosome<Species> Cross(Chromosome<Species> secondPair, Random random = null)
        {
            var newChromosome = GenesFromCrossing(secondPair, random);
            return new SpecieChromosome()
            {
                Property = Property,
                Primary = newChromosome.Item1,
                Secondary = newChromosome.Item2
            };
        }
        
        public static IEnumerable<SpecieCombination> GetPossibleMutations(Species first, Species second, Random random = null)
        {
            random ??= new Random();
            return BeeGeneticDatabase.SpecieCombinations.Where(x => 
                    (x.Parent1 == first && x.Parent2 == second) || 
                    (x.Parent2 == first && x.Parent1 == second));
        }
    }
}