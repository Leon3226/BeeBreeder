using System;
using System.Collections.Generic;
using System.Linq;

namespace BeeBreeder.Common.Model.Bees
{
    public class BeePool
    {
        public List<BeeStack> Bees = new();

        public BeePool()
        {
            CompactDuplicates();
        }

        public BeePool(IEnumerable<Bee> bees)
        {
            foreach (var bee in bees)
            {
                Bees.Add(new BeeStack(bee, 1));
            }
            CompactDuplicates();
        }

        public List<BeeStack> Princesses
        {
            get { return Bees.Where(x => x.Bee.Gender == Gender.Princess).ToList(); }
            set
            {
                Bees.RemoveAll(x => x.Bee.Gender == Gender.Princess);
                Bees.AddRange(value);
            }
        }

        public List<BeeStack> Drones
        {
            get { return Bees.Where(x => x.Bee.Gender == Gender.Drone).ToList(); }
            set
            {
                Bees.RemoveAll(x => x.Bee.Gender == Gender.Drone);
                Bees.AddRange(value);
            }
        }

        public void CompactDuplicates()
        {
            var toCheck = Drones;
            for (int i = 0; i < toCheck.Count; i++)
            {
                if (toCheck.Count == 0)
                    break;
                var bee = toCheck[i];
                var duplicates = toCheck.Except(new[] {bee}).Where(x => x.Bee.Genotype.Equals(bee.Bee.Genotype))
                    .ToArray();
                // duplicates.ForEach(x => bee.Count += x.Count);
                toCheck.Remove(bee);
                i--;
                foreach (var duplicate in duplicates)
                {
                    bee.Count += duplicate.Count;
                    toCheck.Remove(duplicate);
                    Bees.Remove(duplicate); 
                }
            }
        }

        public void RemoveBee(Bee bee, int count = 1)
        {
            var bees = Bees.FirstOrDefault(x => x.Bee == bee);
            if (bees == null) return;
            bees.Count -= count;
            if (bees.Count <= 0)
                Bees.Remove(bees);
        }

        public void RemoveAll(IEnumerable<BeeStack> bees)
        {
            foreach (var deleteStack in bees)
            {
                var beeStack = Bees.FirstOrDefault(x => x.Bee.Equals(deleteStack.Bee));
                if (beeStack != null)
                {
                    beeStack.Count -= deleteStack.Count;
                    if (beeStack.Count <= 0)
                        Bees.Remove(beeStack);
                }
                
            }
            CompactDuplicates();
        }
    }
}