using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using NUnit.Framework;
using StatNames = BeeBreeder.Common.Data.Constants.StatNames;

namespace BeeBreeder.Tests.Breeding
{
    public class Pareto
    {
        private BeeGenerator _generator;
        Bee _bee1;
        Bee _bee2;

        [SetUp]
        public void Setup(BeeGenerator generator)
        {
            _generator = generator;
            _bee1 = _generator.Generate(Species.Forest, Gender.Princess);
            _bee2 = _generator.Generate(Species.Forest, Gender.Princess);

        }

        [Test]
        public void BeeParetoEqualIfEqual()
        {
            Assert.Null(_bee1.ParetoBetter(_bee2));
        }
        
        [Test]
        public void BeeParetoBetterIfOnlyBetter()
        {
            _bee1[StatNames.Speed] = new Chromosome<int>(10 , StatNames.Speed);
            
            Assert.AreEqual(_bee1.ParetoBetter(_bee2), _bee1);
        }
        
        [Test]
        public void BeeParetoEqualIfBothBetterAtSomething()
        {
            _bee1[StatNames.Speed] = new Chromosome<int>(10 , StatNames.Speed);
            _bee2[StatNames.Pollination] = new Chromosome<int>(10 , StatNames.Pollination);
            
            Assert.Null(_bee1.ParetoBetter(_bee2));
        }
    }
}