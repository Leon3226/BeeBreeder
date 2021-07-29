using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BeeBreeder.Breeding.Breeder;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.ProbabilityUtils.Model;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Chance;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
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
            Random rand = new Random();
            var generator = new BeeGenerator();
            IGenomeEvaluator eval = new SumGenomeEvaluator();
            var tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);

            var bee1 = generator.Generate(Species.Forest, Gender.Princess);
            var bee2 = generator.Generate(Species.Meadows);

            var beesChild = bee1.Breed(bee2);

            var pool = new BeePool
            {
                Bees = new List<BeeStack>
                {
                    new(generator.Generate(Species.Forest, Gender.Princess), 1),
                    new(generator.Generate(Species.Forest), 1),
                    new(generator.Generate(Species.Meadows, Gender.Princess), 1),
                    new(generator.Generate(Species.Meadows), 1),
                    new(generator.Generate(Species.Steadfast), 1),
                    new(generator.Generate(Species.Forest, Gender.Princess), 1),
                    new(generator.Generate(Species.Forest), 1),
                    new(generator.Generate(Species.Meadows, Gender.Princess), 1),
                    new(generator.Generate(Species.Meadows), 1)
                }
            };
            pool.CompactDuplicates();

            var pr = tree.PossibleResults(new List<Species>() {Species.Forest, Species.Meadows});

            var targetSpecies = new List<Species>() {Species.Imperial};

            IBeeBreeder randomBreeder = new NaturalSelectionBreeder {TargetSpecies = targetSpecies};
            randomBreeder.Pool = pool;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            TimeSpan timeElapsed = TimeSpan.Zero;

            var breedIterations = 1000;

            int i;
            for (i = 0; i < breedIterations; i++)
            {
                var lifespanGenes = randomBreeder.Pool.Princesses.Select(x =>
                    ((Chromosome<int>) x.Bee.Genotype[StatNames.Lifespan]).ResultantAttribute).Max();
                var lifespan = lifespanGenes * 10 * 28;
                timeElapsed = timeElapsed.Add(new TimeSpan(0, 0, lifespan));

                var values = pool.Bees.Select(x => eval.Evaluate(x.Bee.Genotype));
                randomBreeder.Breed(1);

                //if (values.Average() > 150)
                //    break;
            }

            var es = sw.Elapsed.TotalSeconds;
        }
    }
}