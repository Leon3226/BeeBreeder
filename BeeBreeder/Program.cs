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

            var generator = new BeeGenerator();

            var forestFlyer = new BeeStack(generator.Generate(Species.Forest), 1);
            forestFlyer.Bee[StatNames.Flyer] = new Chromosome<int>(1, StatNames.Flyer);
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