using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BeeBreeder.Breeding.Breeder;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();

            var bee1 = new Bee()
            {
                Gender = Gender.Princess,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Forest])
            };
            var bee2 = new Bee()
            {
                Gender = Gender.Drone,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Meadows])
            };

            var pool = new BeePool()
            {
                Bees = new List<Bee>()
                    {
                        bee1, bee2
                    }
            };

            IBeeBreeder randomBreeder = new RandomBreeder();
            randomBreeder.Pool = pool;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            randomBreeder.Breed(100);
            
            sw.Stop();
            var es = sw.Elapsed.TotalSeconds;

            var sb = new StringBuilder();
            foreach (var item in pool.Bees)
            {
                sb.Append(item);
                sb.Append('\n');
            }

            Console.WriteLine(sb.ToString());
        }
    }
}