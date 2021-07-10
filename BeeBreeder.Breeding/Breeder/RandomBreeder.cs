using System;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Breeder
{
    public class RandomBreeder : IBeeBreeder
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

                Pool.Cross(princesses[_rand.Next(0, princesses.Length)],
                    drones[_rand.Next(0, drones.Length)]);
            }
        }
    }
}