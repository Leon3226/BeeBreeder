using BeeBreeder.Common;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Breeding.Analyzer
{
    public class NaturalSelectionAnalyzer : IBreedAnalyzer
    {
        public List<(Bee, Bee)> GetBreedingPairs(BeePool bees, int count = 0)
        {
            var toReturn = new List<(Bee, Bee)>();

            if (count < 0)
                return toReturn;

            var princesses = bees.Princesses.ToList();
            if (princesses.Count == 0)
                return toReturn;

            var drones = bees.Drones.ToList();
            if (drones.Count == 0)
                return toReturn;

            count = count == 0 ? princesses.Sum(x => x.Count) : count;

            for (int i = 0; i < count; i++)
            {
                if (princesses.Count == 0 || drones.Count == 0)
                    break;
                var princess = princesses[RandomGenerator.GenerateInt(0, princesses.Count)];
                var drone = drones[RandomGenerator.GenerateInt(0, drones.Count)];

                princesses.RemoveCount(princess, 1);
                drones.RemoveCount(drone, 1);

                toReturn.Add((princess.Bee, drone.Bee));
            }

            return toReturn;
        }
    }
}
