using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.WebAPI.Model;

namespace BeeBreeder.WebAPI.Mapping
{
    public static class BeeMapping
    {
        private static Dictionary<string, string> _statNames;
        private static Dictionary<string, int> _areaMapping;
        private static Dictionary<float, int> _speedMapping;
        private static Dictionary<int, int> _lifespanMapping;
        private static Dictionary<int, int> _pollinationMapping;
        private static Dictionary<string, Adaptation> _adaptationMapping;

        static BeeMapping()
        {
            _statNames = new Dictionary<string, string>()
            {
                {"Species", BeeGeneticDatabase.StatNames.Specie},
                {"Effect", BeeGeneticDatabase.StatNames.Effect},
                {"Territory", BeeGeneticDatabase.StatNames.Area},
                {"FlowerProvider", BeeGeneticDatabase.StatNames.Flowers},
                {"CaveDwelling", BeeGeneticDatabase.StatNames.Cave},
                {"TemperatureTolerance", BeeGeneticDatabase.StatNames.TempTolerance},
                {"Flowering", BeeGeneticDatabase.StatNames.Pollination},
                {"Fertility", BeeGeneticDatabase.StatNames.Fertility},
                {"HumidityTolerance", BeeGeneticDatabase.StatNames.HumidTolerance},
                {"Speed", BeeGeneticDatabase.StatNames.Speed},
                {"Lifespan", BeeGeneticDatabase.StatNames.Lifespan},
                {"NeverSleeps", BeeGeneticDatabase.StatNames.Nocturnal}
            };

            _speedMapping = new Dictionary<float, int>()
            {
                {0.3f, 1},
                {0.6f, 2},
                {0.8f, 3},
                {1f, 4},
                {1.2f, 5},
                {1.4f, 6},
                {1.7f, 7}
            };
            
            _lifespanMapping = new Dictionary<int, int>()
            {
                {10, 1},
                {20, 2},
                {30, 3},
                {35, 4},
                {40, 5},
                {45, 6},
                {50, 7},
                {60, 8},
                {70, 9}
            };
            
            _pollinationMapping = new Dictionary<int, int>()
            {
                {5, 1},
                {10, 2},
                {15, 3},
                {20, 4},
                {25, 5},
                {30, 6},
                {35, 7}
            };
            
            _adaptationMapping = new Dictionary<string, Adaptation>()
            {
                {"None", new Adaptation(0, 0)},
                {"Up 1", new Adaptation(1, 0)},
                {"Up 2", new Adaptation(2, 0)},
                {"Down 1", new Adaptation(0, 1)},
                {"Down 2", new Adaptation(0, 1)},
                {"Both 1", new Adaptation(1, 1)},
                {"Both 2", new Adaptation(2, 2)}
            };
            
            _areaMapping = new Dictionary<string, int>()
            {
                {"Vec3i{x=9, y=6, z=9}", 1},
                {"Vec3i{x=11, y=8, z=11}", 2},
                {"Vec3i{x=13, y=12, z=13}", 3},
                {"Vec3i{x=15, y=13, z=15}", 4},
                {"Vec3i{x:9, y:6, z:9}", 1},
                {"Vec3i{x:11, y:8, z:11}", 2},
                {"Vec3i{x:13, y:12, z:13}", 3},
                {"Vec3i{x:15, y:13, z:15}", 4},
            };
        }
            
        public static Bee ToModelBee(this MinecraftBeeModel raw)
        {
            var bee = new Bee();
            bee.Generation = raw.Individual.Generation;
            bee.Gender = raw.Name.Contains("drone")  ? Gender.Drone : Gender.Princess;
            var rawPrimary = raw.Individual.Active;
            var rawSecondary = raw.Individual.Inactive;
            Enum.TryParse(rawPrimary.Species, out Species speciePrimary);
            Enum.TryParse(rawSecondary.Species, out Species specieSecondary);
            Enum.TryParse(rawPrimary.Effect, out Effect effectPrimary);
            Enum.TryParse(rawSecondary.Effect, out Effect effectSecondary);
            Enum.TryParse(rawPrimary.FlowerProvider, out Flowers flowerPrimary);
            Enum.TryParse(rawSecondary.FlowerProvider, out Flowers flowerSecondary);
            
            var specieChromosome = new SpecieChromosome()
            {
                Property = BeeGeneticDatabase.StatNames.Specie,
                Primary = new Gene<Species>() {Value = speciePrimary},
                Secondary = new Gene<Species>() {Value = specieSecondary}
            };
            
            var speedChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Speed,
                Primary = new Gene<int>() {Value = _speedMapping[_speedMapping.Keys.First(x => Math.Abs(x - rawPrimary.Speed) < 0.001)]},
                Secondary = new Gene<int>() {Value = _speedMapping[_speedMapping.Keys.First(x => Math.Abs(x - rawSecondary.Speed) < 0.001)]}
            };
            
            var pollinationChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Pollination,
                Primary = new Gene<int>() {Value = _pollinationMapping[rawPrimary.Flowering]},
                Secondary = new Gene<int>() {Value = _pollinationMapping[rawSecondary.Flowering]}
            };
            
            var areaChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Area,
                Primary = new Gene<int>() {Value = _areaMapping[rawPrimary.Territory]},
                Secondary = new Gene<int>() {Value = _areaMapping[rawSecondary.Territory]}
            };
            
            var lifespanChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Lifespan,
                Primary = new Gene<int>() {Value = _lifespanMapping[rawPrimary.Lifespan]},
                Secondary = new Gene<int>() {Value = _lifespanMapping[rawSecondary.Lifespan]}
            };
            
            var caveChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Cave,
                Primary = new Gene<int>() {Value = Convert.ToInt32(rawPrimary.CaveDwelling)},
                Secondary = new Gene<int>() {Value = Convert.ToInt32(rawSecondary.CaveDwelling)}
            };

            var nocturnalChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Nocturnal,
                Primary = new Gene<int>() {Value = Convert.ToInt32(rawPrimary.NeverSleeps)},
                Secondary = new Gene<int>() {Value = Convert.ToInt32(rawSecondary.NeverSleeps)}
            };
            
            var flyerChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Flyer,
                Primary = new Gene<int>() {Value = Convert.ToInt32(rawPrimary.ToleratesRain)},
                Secondary = new Gene<int>() {Value = Convert.ToInt32(rawSecondary.ToleratesRain)}
            };
            
            var diurnalChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Diurnal,
                Primary = new Gene<int>() {Value = 1},
                Secondary = new Gene<int>() {Value = 1}
            };
            
            var fertilityChromosome = new Chromosome<int>()
            {
                Property = BeeGeneticDatabase.StatNames.Fertility,
                Primary = new Gene<int>() {Value = rawPrimary.Fertility},
                Secondary = new Gene<int>() {Value = rawSecondary.Fertility}
            };
            
            var flowerChromosome = new Chromosome<Flowers>()
            {
                Property = BeeGeneticDatabase.StatNames.Flowers,
                Primary = new Gene<Flowers>() {Value = flowerPrimary},
                Secondary = new Gene<Flowers>() {Value = flowerSecondary}
            };
            
            var effectChromosome = new Chromosome<Effect>()
            {
                Property = BeeGeneticDatabase.StatNames.Effect,
                Primary = new Gene<Effect>() {Value = effectPrimary},
                Secondary = new Gene<Effect>() {Value = effectSecondary}
            };
            
            var humidityChromosome = new Chromosome<Adaptation>()
            {
                Property = BeeGeneticDatabase.StatNames.HumidTolerance,
                Primary = new Gene<Adaptation>() {Value = _adaptationMapping[rawPrimary.HumidityTolerance]},
                Secondary = new Gene<Adaptation>() {Value = _adaptationMapping[rawSecondary.HumidityTolerance]}
            };
            
            var temperatureChromosome = new Chromosome<Adaptation>()
            {
                Property = BeeGeneticDatabase.StatNames.TempTolerance,
                Primary = new Gene<Adaptation>() {Value = _adaptationMapping[rawPrimary.TemperatureTolerance]},
                Secondary = new Gene<Adaptation>() {Value = _adaptationMapping[rawSecondary.TemperatureTolerance]}
            };
            
            
            bee.Genotype.Genes.Add(specieChromosome.Property, specieChromosome);
            bee.Genotype.Genes.Add(lifespanChromosome.Property, lifespanChromosome);
            bee.Genotype.Genes.Add(speedChromosome.Property, speedChromosome);
            bee.Genotype.Genes.Add(pollinationChromosome.Property, pollinationChromosome);
            bee.Genotype.Genes.Add(flowerChromosome.Property, flowerChromosome);
            bee.Genotype.Genes.Add(fertilityChromosome.Property, fertilityChromosome);
            bee.Genotype.Genes.Add(areaChromosome.Property, areaChromosome);
            bee.Genotype.Genes.Add(temperatureChromosome.Property, temperatureChromosome);
            bee.Genotype.Genes.Add(humidityChromosome.Property, humidityChromosome);
            bee.Genotype.Genes.Add(diurnalChromosome.Property, diurnalChromosome);
            bee.Genotype.Genes.Add(nocturnalChromosome.Property, nocturnalChromosome);
            bee.Genotype.Genes.Add(flyerChromosome.Property, flyerChromosome);
            bee.Genotype.Genes.Add(caveChromosome.Property, caveChromosome);
            bee.Genotype.Genes.Add(effectChromosome.Property, effectChromosome);

            foreach (var gene in bee.Genotype.Genes)
            {
                gene.Value.Primary.Dominant = BeeGeneticDatabase.GenesDominancies[gene.Key][gene.Value.Primary.Value];
                gene.Value.Secondary.Dominant = BeeGeneticDatabase.GenesDominancies[gene.Key][gene.Value.Secondary.Value];
            }
            
            return bee;
        }
    }
}