using BeeBreeder.Breeding.Generation;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using NUnit.Framework;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Tests.Breeding
{
    public class ClimateTolerance
    {
        private BeeGenerator _generator;

        [SetUp]
        public void Setup()
        {
            _generator = new BeeGenerator();
        }

        [Test]
        public void CanLiveInAppropriateClimate()
        {
            var bee = _generator.Generate(Species.Forest, Gender.Princess);

            Assert.True(bee.CanLiveIn(Biome.Forest));
        }

        [Test]
        public void CantLiveInInappropriateClimate()
        {
            var bee = _generator.Generate(Species.Forest, Gender.Princess);

            Assert.False(bee.CanLiveIn(Biome.Desert));
        }

        [Test]
        public void CantLiveInInappropriateClimateIfHaveAdaptation()
        {
            var bee = _generator.Generate(Species.Forest, Gender.Princess);
            bee[StatNames.TempTolerance] = new Chromosome<Adaptation>(new Adaptation(2, 0), StatNames.TempTolerance);
            bee[StatNames.HumidTolerance] = new Chromosome<Adaptation>(new Adaptation(0, 1), StatNames.HumidTolerance);

            Assert.True(bee.CanLiveIn(Biome.Desert));
        }
    }
}