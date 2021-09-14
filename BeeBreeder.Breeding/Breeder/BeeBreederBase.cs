using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Breeder
{
    public abstract class BeeBreederBase : IBeeBreeder

    {
        public BeePool Pool { get; set; }
        public bool Compact { get; set; } = false;

        public List<(string, TimeSpan)> _times = new List<(string, TimeSpan)>();

        public List<(string, TimeSpan)> st => _times.OrderByDescending(x => x.Item2).ToList();

        public void Breed(int iterations)
        {
            if (iterations < 0) return;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < iterations;)
            {
                var pairs = GetBreedingPairs();
                _times.Add(("GetPairs", sw.Elapsed));
                sw.Restart();
                if (pairs.Count == 0)
                    break;

                i += pairs.Count;

                pairs.ForEach(x => Pool.Cross(x.Item1, x.Item2));
                _times.Add(("Cross", sw.Elapsed));
                sw.Restart();
                if (Compact)
                {
                    Pool.CompactDuplicates();
                }
                _times.Add(("Compact", sw.Elapsed));
                sw.Restart();

                ToFlush();
                _times.Add(("Flush", sw.Elapsed));
                sw.Restart();
            }
        }

        public abstract List<(Bee, Bee)> GetBreedingPairs(int count = 0);

        public abstract List<(Slot, Bee, Bee)> GetPairsInSlots();

        public abstract IEnumerable<BeeStack> ToFlush();
    }
}