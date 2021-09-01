using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Breeder
{
    public abstract class BeeBreederBase : IBeeBreeder

    {
        public BeePool Pool { get; set; }
        public bool Compact { get; set; } = false;

        public void Breed(int iterations)
        {
            if (iterations < 0) return;
            for (int i = 0; i < iterations;)
            {
                var pairs = GetBreedingPairs();
                if (pairs.Count == 0)
                    break;
                
                i += pairs.Count;

                pairs.ForEach(x => Pool.Cross(x.Item1, x.Item2));
                if (Compact)
                {
                    Pool.CompactDuplicates();
                }
                ToFlush();
            }
        }

        public abstract List<(Bee, Bee)> GetBreedingPairs(int count = 0);

        public abstract List<(Slot, Bee, Bee)> GetPairsInSlots();

        public abstract IEnumerable<BeeStack> ToFlush();
    }
}