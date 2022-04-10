using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth
{
    public class BreedingTarget
    {
        public bool PrioritizeSpecies = true;
        public Dictionary<Species, int> SpeciePriorities = new();
        public bool PrioritizeFlowers;
        public Dictionary<Species, int> FlowersPriorities = new();

        public BreedingTarget Copy()
        {
            return new BreedingTarget()
            {
                PrioritizeSpecies = PrioritizeSpecies,
                SpeciePriorities = SpeciePriorities.ToDictionary(x => x.Key, x => x.Value),
                PrioritizeFlowers = PrioritizeFlowers,
                FlowersPriorities = SpeciePriorities.ToDictionary(x => x.Key, x => x.Value),
            };
        }
    }
}