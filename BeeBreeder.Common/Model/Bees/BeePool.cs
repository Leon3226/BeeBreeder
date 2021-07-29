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
        public List<BeeStack> Princesses
        {
            get
            {
                return Bees.Where(x => x.Bee.Gender == Gender.Princess).ToList();
            }
            set
            {
                Bees.RemoveAll(x => x.Bee.Gender == Gender.Princess);
                Bees.AddRange(value);
            }
        }
        public List<BeeStack> Drones
        {
            get
            {
                return Bees.Where(x => x.Bee.Gender == Gender.Drone).ToList();
            }
            set
            {
                Bees.RemoveAll(x => x.Bee.Gender == Gender.Drone);
                Bees.AddRange(value);
            }
        }

        public void CompactDuplicates()
        {
            for (int i = 0; i < Drones.Count; i++)
            {
                var bee = Drones[i];
                var duplicates = Drones.Where(x => x.Bee.Genotype.Equals(bee.Bee.Genotype)).Except(new[] {bee}).ToList();
                duplicates.ForEach(x => bee.Count += x.Count);
                foreach (var duplicate in duplicates)
                {
                    Bees.Remove(duplicate);
                }
            }
        }

        public void RemoveBee(Bee bee, int count)
        {
            var bees = Bees.FirstOrDefault(x => x.Bee == bee);
            if (bees == null) return;
            bees.Count -= count;
            if (bees.Count <= 0)
                Bees.Remove(bees);
        }

        public bool Cross(Bee first, Bee second)
        {
            var child = first.Breed(second);
            if (child == null)
                return false;
            
            Bees.AddRange(child.Select(x => new BeeStack(x, 1)));
            return true;
        }
    }
}