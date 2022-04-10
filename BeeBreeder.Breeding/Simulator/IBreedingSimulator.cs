using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Simulator
{
    public interface IBreedingSimulator
    {
        BeePool Pool { get; set; }
        void Breed(int iterations);
    }
}
