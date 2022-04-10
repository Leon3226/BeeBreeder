using System;
using System.Collections.Generic;
using System.Diagnostics;
using BeeBreeder.Breeding;
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

            var forestFlyer = new BeeStack(generator.Generate(Species.Forest), 1)
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
                    new(generator.Generate(Species.Forest, Gender.Princess), 8),
                    new(generator.Generate(Species.Forest), 8),
                    new(generator.Generate(Species.Meadows, Gender.Princess), 8),
                    new(generator.Generate(Species.Meadows), 8),
                    new(generator.Generate(Species.Steadfast), 1),
                    new(generator.Generate(Species.Tropical), 1),
                    new(generator.Generate(Species.Modest), 1),
                    new(generator.Generate(Species.Modest, Gender.Princess),  1),
                    new(generator.Generate(Species.Tropical, Gender.Princess),  1),
                    forestFlyer
                }
            };
            sim.Pool.CompactDuplicates();
            var sw = new Stopwatch();
            sw.Start();

            var breedIterations = 1000;
            for (int j = 0; j < breedIterations; j++)
            {
                sim.Breed(1);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}