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
        }
        public List<Bee> Drones
        {
            get
            {
                return Bees.Where(x => x.Gender == Gender.Drone).ToList();
            }
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