using System.Collections.Generic;
using System.Linq;

namespace BeeBreeder.Common.Model.Bees
{
    public class BeePool
    {
        public List<Bee> Bees = new();

        public List<Bee> Princesses
        {
            get
            {
                return Bees.Where(x => x.Gender == Gender.Princess).ToList();
            }
            set
            {
                Bees.RemoveAll(x => x.Gender == Gender.Princess);
                Bees.AddRange(value);
            }
        }
        public List<Bee> Drones
        {
            get
            {
                return Bees.Where(x => x.Gender == Gender.Drone).ToList();
            }
            set
            {
                Bees.RemoveAll(x => x.Gender == Gender.Drone);
                Bees.AddRange(value);
            }
        }

        public int RemoveDroneDuplicates(int targetDuplicatesCount = 0)
        {
            var dronesToCheck = Drones.ToList();
            var originalDrones = new List<Bee>();
            for (int i = 0; i < dronesToCheck.Count; i++)
            {
                var bee = dronesToCheck[i];
                var duplicates = dronesToCheck.Where(x => x.Genotype.Equals(bee.Genotype)).ToList();
                originalDrones.AddRange(duplicates.Take(targetDuplicatesCount + 1));
                dronesToCheck = dronesToCheck.Except(duplicates).ToList();
            }

            var diff = Drones.Count - originalDrones.Count;
            Drones = originalDrones;
            return diff;
        }

        public bool Cross(Bee first, Bee second)
        {
            if (!Bees.Contains(first) || !Bees.Contains(second))
                return false;

            var child = first.Breed(second);
            if (child == null)
                return false;
            
            Bees.AddRange(child);
            Bees.Remove(first);
            Bees.Remove(second);
            return true;
        }
    }
}