using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using NUnit.Framework;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Tests.Breeding
{
    public class Pareto
    {
        private BeeGenerator _generator;
        
        [SetUp]
        public void Setup()
        {
            _generator = new BeeGenerator();
        }

        [Test]
        public void BeeParetoEqualIfEqual()
        {
            var bee1 = _generator.Generate(Species.Forest, Gender.Princess);
            var bee2 = _generator.Generate(Species.Forest, Gender.Princess);
            
            Assert.Null(bee1.ParetoBetter(bee2));
        }
        
        [Test]
        public void BeeParetoBetterIfOnlyBetter()
        {
            var bee1 = _generator.Generate(Species.Forest, Gender.Princess);
            var bee2 = _generator.Generate(Species.Forest, Gender.Princess);
            bee1[StatNames.Speed] = new Chromosome<int>(10 , StatNames.Speed);
            
            Assert.AreEqual(bee1.ParetoBetter(bee2), bee1);
        }
        
        [Test]
        public void BeeParetoEqualIfBothBetterAtSomething()
        {
            var bee1 = _generator.Generate(Species.Forest, Gender.Princess);
            var bee2 = _generator.Generate(Species.Forest, Gender.Princess);
            bee1[StatNames.Speed] = new Chromosome<int>(10 , StatNames.Speed);
            bee2[StatNames.Pollination] = new Chromosome<int>(10 , StatNames.Pollination);
            
            Assert.Null(bee1.ParetoBetter(bee2));
        }
        
        [Test]
        public void BeePareto()
        {
        }
    }
}