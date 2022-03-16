using System.Collections.Generic;
using System.Linq;
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
    }
}