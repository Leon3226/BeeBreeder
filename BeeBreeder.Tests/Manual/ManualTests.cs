using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.Breeder;
using BeeBreeder.Breeding.Generation;
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

        //[Test]
        public void PickCoefficients()
        {
            var targetValue = 130;
            var maxBreeds = 1200;
            var fromNaturalSelection = 1;
            var toNaturalSelection = 50;
            var iterations = 5;
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

            var avv = averageValues.Select(x => (x.Key, x.Value.Count(v => v.Value.Item1), x.Value.Average(v => v.Value.Item2))).OrderByDescending(x=> x.Item3).ToList();
        }

        private BeePool GetStartPool()
        {
            var generator = new BeeGenerator();

            return new BeePool()
            {
                Bees = new List<Bee>()
                {
                    generator.Generate(Species.Forest, Gender.Princess),
                    generator.Generate(Species.Forest),
                    generator.Generate(Species.Meadows, Gender.Princess),
                    generator.Generate(Species.Meadows),
                    generator.Generate(Species.Steadfast)
                }
            };
        }
    }
}