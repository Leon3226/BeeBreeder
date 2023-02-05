using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using BeeBreeder.Breeding;
using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Breeding.Comparison.Gene.Comparators;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using BeeBreeder.Common.Model.Positioning;
using BeeBreeder.Extensions;
using BeeBreeder.Management.Manager;
using Microsoft.Extensions.DependencyInjection;
using StatNames = BeeBreeder.Common.Data.Constants.StatNames;

namespace BeeBreeder
{
    class Program
    {
        static void Main(string[] args)
        {

            var st = "One.Two.Three.Four.";
            var asd = st.Split('.', 2);

            var servideProvider = new ServiceCollection().AddBeeBreeder()
                .AddSocketManaging(new IPEndPoint(IPAddress.Any, 8005)).BuildServiceProvider();

            var scp = servideProvider.GetService<ISpecieCombinationsProvider>();
            var a = scp.SpecieCombinations;

            var manager = servideProvider.GetService<SimpleManager>();
            
            Thread.Sleep(12000); //Until all will connect
            manager.ComputerNames = new string[] { "njTDEbHW", "YXFaE3w8", "AnVvDHUl" };
            manager.LoadData();
            var compPlains = manager.Computers.SingleOrDefault(x => x.Identifier == "njTDEbHW");
            var compTundra = manager.Computers.SingleOrDefault(x => x.Identifier == "YXFaE3w8");
            var compDesert = manager.Computers.SingleOrDefault(x => x.Identifier == "AnVvDHUl");
            if (compPlains != null)
            {
                foreach (var transposer in compPlains.Trasposers)
                {
                    transposer.Roofed = false;
                    transposer.Biome = Biome.Meadow;
                    transposer.Flowers = new string[] { "Flowers" };
                }
            }
            
            if (compTundra != null)
            {
                foreach (var transposer in compTundra.Trasposers)
                {
                    transposer.Roofed = false;
                    transposer.Biome = Biome.SnowForest;
                    transposer.Flowers = new string[] { "Flowers", "Snow" };
                }
            }
            
            if (compDesert != null)
            {
                foreach (var transposer in compDesert.Trasposers)
                {
                    transposer.Roofed = false;
                    transposer.Biome = Biome.Desert;
                    transposer.Flowers = new string[] { "Cacti" };
                }
            }
            
            //manager.PlaceBees();

            var matcher = servideProvider.GetService<IPositionsController>();

            var transposerBiomes = new List<TransposerData>()
            {
                new TransposerData() { Biome = Common.Model.Environment.Biome.Forest, Transposer = "1a", Flowers = new []{ "Forest" }, IsRoofed = false},
                new TransposerData() { Biome = Common.Model.Environment.Biome.Jungle, Transposer = "2c", Flowers = new []{ "Jungle" }, IsRoofed = false}
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

            var sw = new Stopwatch();
            sw.Start();

            var breedIterations = 5000;
            sim.Breed(breedIterations);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}