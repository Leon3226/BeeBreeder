using System;
using System.Collections.Generic;
using System.Diagnostics;
using BeeBreeder.Breeding;
using BeeBreeder.Breeding.Comparison.Gene.Comparators;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using Microsoft.Extensions.DependencyInjection;
using StatNames = BeeBreeder.Common.Data.Constants.StatNames;

namespace BeeBreeder
{
    class Program
    {
        static void Main(string[] args)
        {
            var servideProvider = new ServiceCollection().AddBeeBreeder().BuildServiceProvider();

            var generator = servideProvider.GetService<BeeGenerator>();
            var sim = servideProvider.GetService<IBreedingSimulator>();

            var forestFlyer = new BeeStack(generator.Generate("Forest"), 1)
            {
                Bee =
                {
                    [StatNames.Flyer] = new Chromosome<int>(1, StatNames.Flyer)
                }
            };
            sim.Pool = new BeePool
            {
                Bees = new List<BeeStack>
                {
                    new(generator.Generate("Forest", Gender.Princess), 8),
                    new(generator.Generate("Forest"), 8),
                    new(generator.Generate("Meadows", Gender.Princess), 8),
                    new(generator.Generate("Meadows"), 8),
                    new(generator.Generate("Steadfast"), 1),
                    new(generator.Generate("Tropical"), 1),
                    new(generator.Generate("Modest"), 1),
                    new(generator.Generate("Modest", Gender.Princess),  1),
                    new(generator.Generate("Tropical", Gender.Princess),  1),
                    forestFlyer
                }
            };
            sim.Pool.CompactDuplicates();
            var sw = new Stopwatch();
            sw.Start();

            var breedIterations = 4000;
            sim.Breed(breedIterations);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}