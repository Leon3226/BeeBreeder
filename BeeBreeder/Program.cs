using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BeeBreeder.Breeding;
using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Breeding.Comparison.Gene.Comparators;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using BeeBreeder.Common.Model.Positioning;
using BeeBreeder.Extensions;
using Microsoft.Extensions.DependencyInjection;
using StatNames = BeeBreeder.Common.Data.Constants.StatNames;

namespace BeeBreeder
{
    class Program
    {
        static void Main(string[] args)
        {
            var servideProvider = new ServiceCollection().AddBeeBreeder().BuildServiceProvider();
            var matcher = servideProvider.GetService<IPositionsController>();

            var transposerBiomes = new List<TransposerData>()
            {
                new TransposerData() { Biome = Common.Model.Environment.Biome.Forest, Transposer = "1a", Flowers = new []{ "Forest" }, IsRoofed = false},
                new TransposerData() { Biome = Common.Model.Environment.Biome.Jungle, Transposer = "2c", Flowers = new []{ "Jungle" }, IsRoofed = false}
            };

            var avaliablePositions = new List<ApiaryPosition>()
            {
                new ApiaryPosition() {Side = 2, Slot = 1, Trans = "2c"},
                new ApiaryPosition() {Side = 3, Slot = 1, Trans = "2c"},
                new ApiaryPosition() {Side = 4, Slot = 1, Trans = "2c"},
                new ApiaryPosition() {Side = 2, Slot = 1, Trans = "1a"},
                new ApiaryPosition() {Side = 3, Slot = 1, Trans = "1a"},
                new ApiaryPosition() {Side = 4, Slot = 1, Trans = "1a"},
            };

            var analyzer = servideProvider.GetService<IBreedAnalyzer>();

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

            var pairs = analyzer.GetBreedingPairs(sim.Pool);
            var positionedPairs = matcher.Assign(pairs, transposerBiomes, avaliablePositions);

            var sw = new Stopwatch();
            sw.Start();

            //var breedIterations = 4000;
            //sim.Breed(breedIterations);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}