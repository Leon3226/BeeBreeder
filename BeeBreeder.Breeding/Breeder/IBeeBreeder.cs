using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Breeder
{
    public interface IBeeBreeder 
    {   
        BeePool Pool { get; set; }
        void Breed(int iterations);
    }
}