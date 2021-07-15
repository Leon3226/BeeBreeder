using System;
using System.Linq;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Chance;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Breeder
{
    public class ChanceBreeder : IBeeBreeder
    {
        readonly Random _rand = new();
        public BeePool Pool { get; set; }

        public void Breed(int iterations)
        {
            if (iterations < 0) return;
            for (int i = 0; i < iterations; i++)
            {
                var princesses = Pool.Princesses.ToArray();
                if (princesses.Length == 0)
                    break;
                var drones = Pool.Drones.ToArray();
                if (drones.Length == 0)
                    break;
                var princess = princesses[_rand.Next(0, princesses.Length)];
                var probabilities = drones.Select(drone => (drone, new BeeCrossChance(princess, drone)));
                var drone = probabilities.OrderByDescending(x =>
                {
                    var princessChances = BeeChangeChanceModel.GetChances(princess, x.Item2);
                    var droneChances = BeeChangeChanceModel.GetChances(x.drone, x.Item2);

                    var chanceToImprove = princessChances.Sum(chance => chance.Value.ChanceToImprove) +
                                          droneChances.Sum(chance => chance.Value.ChanceToImprove);
                    
                    var chanceToSpoil = princessChances.Sum(chance => chance.Value.ChanceToSpoil) +
                                        droneChances.Sum(chance => chance.Value.ChanceToSpoil);

                    return chanceToImprove - chanceToSpoil;
                }).First();

                Pool.Cross(princess,drone.drone);
            }
        }
    }
}