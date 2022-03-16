using BeeBreeder.Common.Model.Genetics;
using NUnit.Framework;

namespace BeeBreeder.Tests.Genetics
{
    public class GeneticsTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ChromosomesAreCrossingCorrectly()
        {
            var chromosome1 = new Chromosome<int>(1);
            var chromosome2 = new Chromosome<int>(2);

            var chromosome3 = (Chromosome<int>)chromosome1.Cross(chromosome2);

            Assert.IsTrue(chromosome3.Primary.Value == 1 && chromosome3.Secondary.Value == 2 || chromosome3.Primary.Value == 2 && chromosome3.Secondary.Value == 1);
        }
    }
}