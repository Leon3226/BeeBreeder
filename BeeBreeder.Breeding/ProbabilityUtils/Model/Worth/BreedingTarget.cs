using System.Collections.Generic;
using BeeBreeder.Common.AlleleDatabase.Bee;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth
{
    public class BreedingTarget
    {
        public bool PrioritizeSpecies;
        public Dictionary<Species, int> SpeciePriorities = new();
        public bool PrioritizeFlowers;
        public Dictionary<Species, int> FlowersPriorities = new(); 
    }
}