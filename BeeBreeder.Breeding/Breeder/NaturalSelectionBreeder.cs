using System;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Paretho;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Breeder
{
    public class NaturalSelectionBreeder : IBeeBreeder
    {
        readonly Random _rand = new();

        public int IterationsBetweenNaturalSelectionClears = 50;
        public bool ClearDuplicates = true;
        public int IterationsBetweenDuplicatesClears = 50;
        public int AllowedDuplicatesCount = 3;
        public BeePool Pool { get; set; }

        public void Breed(int iterations)
        {
            if (iterations < 0) return;
            for (int i = 0; i < iterations; i++)
            {
                if (i != 0 && i % IterationsBetweenDuplicatesClears == 0)
                {
                    Pool.RemoveDroneDuplicates(AllowedDuplicatesCount);
                }
                if (i != 0 && i % IterationsBetweenNaturalSelectionClears == 0)
                {
                    NaturalSelection();
                }
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

        public int NaturalSelection()
        {
            var optimalDrones = Pool.Drones.ParethoOptimal();
            var count = Pool.Drones.Count - optimalDrones.Count;
            Pool.Drones = optimalDrones;
            return count;
        }
    }
}