using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Comparators;
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