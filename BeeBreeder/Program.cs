using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BeeBreeder.Breeding.Breeder;
using BeeBreeder.Breeding.ProbabilityUtils.Model;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Chance;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Paretho;
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

            var bee1 = new Bee()
            {
                Gender = Gender.Princess,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Forest])
            };
            var bee2 = new Bee()
            {
                Gender = Gender.Princess,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Meadows])
            };
            var bee3 = new Bee()
            {
                Gender = Gender.Princess,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Meadows])
            };
            var bee4 = new Bee()
            {
                Gender = Gender.Drone,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Forest])
            };
            var bee5 = new Bee()
            {
                Gender = Gender.Drone,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Steadfast])
            };
            var bee6 = new Bee()
            {
                Gender = Gender.Princess,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Meadows])
            };
            var bee7 = new Bee()
            {
                Gender = Gender.Princess,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Tropical])
            };
            var bee8 = new Bee()
            {
                Gender = Gender.Drone,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Tropical])
            };

            var pl = new List<Bee>() {bee1, bee2, bee3};

            var opt = pl.ParethoOptimal();

            var pb = bee1.ParethoBetter(bee2);

            var dg = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Imperial]);

            var pool = new BeePool()
            {
                Bees = new List<Bee>()
                {
                    bee1, bee2, bee3, bee4, bee5, bee6
                }
            };

            var tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);
            
            var b1 = tree[Species.Forest].LeadsTo(Species.Imperial);
            var b2 = tree[Species.Common].LeadsTo(Species.Imperial);
            var b3 = tree[Species.Tropical].LeadsTo(Species.Imperial);
            var b4 = tree[Species.Rural].LeadsTo(Species.Imperial);

            var a1 = (Chromosome<Species>) pool.Bees[0].Genotype[BeeGeneticDatabase.StatNames.Specie];
            var a2 = (Chromosome<Species>) pool.Bees[1].Genotype[BeeGeneticDatabase.StatNames.Specie];
            var common = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Common]);
            var cultivated = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Cultivated]);

            var bcc = new BeeCrossChance(bee2, bee4);
            var chances = BeeChangeChanceModel.GetChances(bee2, bcc);

            IBeeBreeder randomBreeder = new NaturalSelectionBreeder();
            randomBreeder.Pool = pool;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            TimeSpan timeElapsed = TimeSpan.Zero;
            
            var targetSpecies = new List<Species>() {Species.Industrious, Species.Imperial};
            var breedIterations = 500;

            var timeQuants = breedIterations / randomBreeder.Pool.Princesses.Count;
            var lifespanGenes = randomBreeder.Pool.Princesses.Select(x =>
                ((Chromosome<int>) x.Genotype[BeeGeneticDatabase.StatNames.Lifespan]).ResultantAttribute).Max();
            var lifespan = lifespanGenes * 10 * 28 * timeQuants;
            timeElapsed = timeElapsed.Add(new TimeSpan(0, 0, lifespan));
          
            randomBreeder.Breed(breedIterations);
            
            var es = sw.Elapsed.TotalSeconds;
            sw.Reset();

            var parethoEs = sw.Elapsed.TotalSeconds;

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