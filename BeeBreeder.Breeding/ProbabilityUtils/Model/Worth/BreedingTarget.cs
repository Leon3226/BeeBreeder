using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth
{
    public class BreedingTarget
    {
        public bool PrioritizeSpecies = true;
        public Dictionary<Species, int> SpeciePriorities;
        public bool PrioritizeFlowers;
        public Dictionary<Species, int> FlowersPriorities = new();

        public BreedingTarget()
        {
            SpeciePriorities = BeeGeneticDatabase.DefaultSpeciePriorities.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}