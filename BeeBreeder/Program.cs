using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Autofac;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.ProbabilityUtils.Model;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Chance;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = DIConfig.BuildContainer();
            var sim = container.Resolve<IBreedingSimulator>();

            Random rand = new Random(23213);
            var generator = new BeeGenerator();
            IGenomeEvaluator eval = new SumGenomeEvaluator();
            var tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);

            var slots = new Slot[]
            {
                new() { Address = "1", Biome = Biome.Forest, EmptySlots = 5},
                new() { Address = "2", Biome = Biome.Forest, EmptySlots = 5},
                new() { Address = "3", Biome = Biome.Jungle, EmptySlots = 5},
                new() { Address = "4", Biome = Biome.Desert, EmptySlots = 5},
                new() { Address = "5", Biome = Biome.Meadow, EmptySlots = 5}
            };

            var bee1 = generator.Generate(Species.Forest, Gender.Princess);
            var bee2 = generator.Generate(Species.Meadows);

            var g = tree.PossibleResults(new List<Species>() {Species.Cultivated, Species.Noble});

            var beesChild = bee1.Breed(bee2);
            var forestFlyer = new BeeStack(generator.Generate(Species.Forest), 1);
            forestFlyer.Bee[StatNames.Flyer] = new Chromosome<int>(1, StatNames.Flyer);
            var pool = new BeePool
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
            pool.CompactDuplicates();

            var valA = 1;
            var valB = Species.Austere;

            var list = new List<object>() {valA, valB};
            foreach (var item in list)
            {
                var type = item.GetType();
                var genericType = typeof(Gene<>).MakeGenericType(type);
                var c = Activator.CreateInstance(genericType, item);
            }

            var pr = tree.PossibleResults(new List<Species>() {Species.Forest, Species.Meadows});

            var targetSpecies = new List<Species>() {Species.Exotic, Species.Imperial, Species.Industrious, Species.Rural};
            Stopwatch sw = new Stopwatch();
            sw.Start();

            TimeSpan timeElapsed = TimeSpan.Zero;


            Dictionary<Species, Dictionary<int, int>> dataTable = new();

            var allSpecies = BeeGeneticDatabase.SpecieStats.Select(x => x.Key).ToArray();
            foreach (var spec in allSpecies)
            {
                dataTable.Add(spec, new Dictionary<int, int>());
            }

            void WriteData(int iter)
            {
                foreach (var spec in allSpecies)
                {
                    var count = pool.Bees.Where(x => x.Bee.SpecieChromosome.Primary.Value == spec)
                        .Sum(x => x.Count);
                    count += pool.Bees.Where(x => x.Bee.SpecieChromosome.Secondary.Value == spec)
                        .Sum(x => x.Count);
                    dataTable[spec][iter] = count;
                }
            }

            string dataString()
            {
                var sb = new StringBuilder();
                foreach (var row in dataTable)
                {
                    sb.Append(row.Key.ToString());
                    sb.Append(",");
                    foreach (var column in row.Value)
                    {
                        sb.Append(column.Value);
                        sb.Append(",");
                    }

                    sb.Append("\n");
                }

                return sb.ToString();
            }

            var ens = Enum.GetValues(typeof(Species));
            var adp1 = pool.Bees.Select(x => x.Bee.AcceptableConditions).ToList();
            var biomes1 = pool.Bees.Select(x => x.Bee.AcceptableBiomes).ToList();

            var data = new StringBuilder();
            int i;
            double lastAverageValue;
            var breedIterations = 10000;

            sim.Pool = pool;

            for (int j = 0; j < 1000; j++)
            {
                sim.Breed(1);
            }

            //for (i = 0; i < breedIterations; i++)
            //{
            //    var lifespanGenes = randomBreeder.Pool.Princesses.Select(x =>
            //        x.Bee.ChromosomeOf<int>(StatNames.Lifespan).ResultantAttribute).Max();
            //    var lifespan = lifespanGenes * 10 * 28;
            //    timeElapsed = timeElapsed.Add(new TimeSpan(0, 0, lifespan));

            //    var values = pool.Drones.Select(x => eval.Evaluate(x.Bee.Genotype));
            //    randomBreeder.Breed(1);
            //    WriteData(i);

            //    var adp = pool.Bees.Select(x => x.Bee.AcceptableConditions).ToList();
            //    var biomes = pool.Bees.Select(x => x.Bee.AcceptableBiomes).ToList();

            //    lastAverageValue = values.Average();
            //    if (lastAverageValue >= 240)
            //    {   
            //        //Console.WriteLine(lastAverageValue);
            //        //break;    
            //    }
            //}

            var es = sw.Elapsed.TotalSeconds;
            var d = dataString();
            Console.WriteLine(es);
            Console.WriteLine(ParetoExtensions.Comparisons);
        }
    }
}