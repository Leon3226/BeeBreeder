using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data.Model
{
    public class SpecieCombination
    {
        public readonly string Parent1;
        public readonly string Parent2;
        public readonly double MutationChance;
        public readonly string MutationResult;

        public SpecieCombination(string parent1, string parent2, double mutationChance, string mutationResult)
        {
            Parent1 = parent1;
            Parent2 = parent2;
            MutationChance = mutationChance;
            MutationResult = mutationResult;
        }
    }
}