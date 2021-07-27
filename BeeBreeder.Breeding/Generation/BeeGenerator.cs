using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Generation
{
    public class BeeGenerator
    {
        public Bee Generate(Species specie, Gender gender = Gender.Drone)
        {
            return new()
            {
                Gender = gender,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[specie])
            };
        }
    }
}