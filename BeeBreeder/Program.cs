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
            
            var pool = new BeePool
            {
                Bees = new List<Bee>
                {
                    generator.Generate(Species.Forest, Gender.Princess),
                    generator.Generate(Species.Forest),
                    generator.Generate(Species.Meadows, Gender.Princess),
                    generator.Generate(Species.Meadows),
                    generator.Generate(Species.Steadfast),
                    generator.Generate(Species.Forest, Gender.Princess),
                    generator.Generate(Species.Forest),
                    generator.Generate(Species.Meadows, Gender.Princess),
                    generator.Generate(Species.Meadows)
                }
            };

            var targetSpecies = new List<Species>() {Species.Industrious, Species.Imperial};

            IBeeBreeder randomBreeder = new NaturalSelectionBreeder {TargetSpecies = targetSpecies};
            randomBreeder.Pool = pool;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            TimeSpan timeElapsed = TimeSpan.Zero;

            var breedIterations = 1000;

            var timeQuantum = breedIterations / randomBreeder.Pool.Princesses.Count;
            var lifespanGenes = randomBreeder.Pool.Princesses.Select(x =>
                ((Chromosome<int>) x.Genotype[BeeGeneticDatabase.StatNames.Lifespan]).ResultantAttribute).Max();
            var lifespan = lifespanGenes * 10 * 28 * timeQuantum;
            timeElapsed = timeElapsed.Add(new TimeSpan(0, 0, lifespan));

            randomBreeder.Breed(breedIterations);

            var es = sw.Elapsed.TotalSeconds;
        }
    }
}