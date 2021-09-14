using System;
using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Breeder
{
    public interface IBeeBreeder 
    {   
        BeePool Pool { get; set; }
        void Breed(int iterations);
        List<(Bee,Bee)> GetBreedingPairs(int count = 0);
        List<(Slot, Bee, Bee)> GetPairsInSlots();
        IEnumerable<BeeStack> ToFlush();
    }
}