using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.Breeder;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using NUnit.Framework;

namespace BeeBreeder.Tests.Manual
{
    public class ManualTests
    {
        private IGenomeEvaluator _evaluator;
        
        [SetUp]
        public void Setup()
        {
            _evaluator = new SumGenomeEvaluator();
        }

        [Test]
        public void PickCoefficients()
        {
            var targetValue = 130;
            var maxBreeds = 1200;
            var fromNaturalSelection = 30;
            var toNaturalSelection = 60;
            var iterations = 10;
            var averageValues = new Dictionary<int, Dictionary<int, (bool, int)>>();

            for (int i = fromNaturalSelection; i < toNaturalSelection; i++)
            {
                var nd = new Dictionary<int, (bool, int)>();
                for (int j = 0; j < iterations; j++)
                {
                    IBeeBreeder breeder = new NaturalSelectionBreeder()
                        {IterationsBetweenDuplicatesClears = 0, IterationsBetweenNaturalSelectionClears = i, Pool = GetStartPool()};

                    double averageValue = 0;
                    for (int k = 0; k < maxBreeds; k++)
                    {
                        breeder.Breed(1);
                        averageValue = breeder.Pool.Bees.Select(x => _evaluator.Evaluate(x.Genotype)).Average();
                        if (averageValue > targetValue)
                        {
                            nd.Add(j, (true, k));
                            break;
                        }
                    }

                    if (averageValue < targetValue)
                    {
                        nd.Add(j, (false, maxBreeds));
                    }
                }
                averageValues.Add(i, nd);
            }
        }

        private BeePool GetStartPool()
        {
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
                Gender = Gender.Drone,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Forest])
            };
            var bee4 = new Bee()
            {
                Gender = Gender.Drone,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Steadfast])
            };
            var bee5 = new Bee()
            {
                Gender = Gender.Drone,
                Genotype = Genotype.FromInitialStats(BeeGeneticDatabase.SpecieStats[Species.Meadows])
            };

            return new BeePool()
            {
                Bees = new List<Bee>()
                {
                    bee1, bee2, bee3, bee4, bee5
                }
            };
        }
    }
}