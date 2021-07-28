using BeeBreeder.Breeding.Generation;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using NUnit.Framework;

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
            bee.Genotype.Genes[BeeGeneticDatabase.StatNames.TempTolerance] = new Chromosome<Adaptation>()
            {
                Property = BeeGeneticDatabase.StatNames.TempTolerance,
                Primary = new Gene<Adaptation>() {Value = new Adaptation(2, 0)},
                Secondary = new Gene<Adaptation>() {Value = new Adaptation(2, 0)}
            };
            bee.Genotype.Genes[BeeGeneticDatabase.StatNames.HumidTolerance] = new Chromosome<Adaptation>()
            {
                Property = BeeGeneticDatabase.StatNames.HumidTolerance,
                Primary = new Gene<Adaptation>() {Value = new Adaptation(0, 1)},
                Secondary = new Gene<Adaptation>() {Value = new Adaptation(0, 1)}
            };
            
            Assert.True(bee.CanLiveIn(Biome.Desert));
        }
    }
}