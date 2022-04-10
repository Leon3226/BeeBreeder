using BeeBreeder.Breeding.EnvironmentMatching;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using NUnit.Framework;
using StatNames = BeeBreeder.Common.Data.Constants.StatNames;

namespace BeeBreeder.Tests.Breeding
{
    public class ClimateTolerance
    {
        private BeeGenerator _generator;
        private IEnvironmentMatcher _environmentMatcher;

        [SetUp]
        public void Setup(BeeGenerator generator, IEnvironmentMatcher environmentMatcher)
        {
            _generator = generator;
            _environmentMatcher = environmentMatcher;
        }

        [Test]
        public void CanLiveInAppropriateClimate()
        {
            var bee = _generator.Generate(Species.Forest, Gender.Princess);

            Assert.True(_environmentMatcher.CanLiveIn(bee, Biome.Forest));
        }

        [Test]
        public void CantLiveInInappropriateClimate()
        {
            var bee = _generator.Generate(Species.Forest, Gender.Princess);

            Assert.True(_environmentMatcher.CanLiveIn(bee, Biome.Desert));
        }

        [Test]
        public void CantLiveInInappropriateClimateIfHaveAdaptation()
        {
            var bee = _generator.Generate(Species.Forest, Gender.Princess);
            bee[StatNames.TempTolerance] = new Chromosome<Adaptation>(new Adaptation(2, 0), StatNames.TempTolerance);
            bee[StatNames.HumidTolerance] = new Chromosome<Adaptation>(new Adaptation(0, 1), StatNames.HumidTolerance);

            Assert.True(_environmentMatcher.CanLiveIn(bee, Biome.Desert));
        }
    }
}