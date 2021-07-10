namespace BeeBreeder.Common.AlleleDatabase.Bee
{
    public class SpecieCombination
    {
        public readonly Species Parent1;
        public readonly Species Parent2;
        public readonly double MutationChance;
        public readonly Species MutationResult;

        public SpecieCombination(Species parent1, Species parent2, double mutationChance, Species mutationResult)
        {
            Parent1 = parent1;
            Parent2 = parent2;
            MutationChance = mutationChance;
            MutationResult = mutationResult;
        }
    }
}