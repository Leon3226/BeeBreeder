using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using BeeBreeder.WebAPI.Model;
using StatNames = BeeBreeder.Common.Data.Constants.StatNames;

namespace BeeBreeder.WebAPI.Mapping
{
    //TODO: Make NonStatic
    public static class BeeMapping
    {
        private static Dictionary<string, string> _statNames;
        private static readonly Dictionary<string, int> AreaMapping;
        private static readonly Dictionary<float, int> SpeedMapping;
        private static readonly Dictionary<int, int> LifespanMapping;
        private static readonly Dictionary<int, int> PollinationMapping;
        private static readonly Dictionary<string, Adaptation> AdaptationMapping;

        static BeeMapping()
        {
            _statNames = new Dictionary<string, string>()
            {
                {"Species", StatNames.Specie},
                {"Effect", StatNames.Effect},
                {"Territory", StatNames.Area},
                {"FlowerProvider", StatNames.Flowers},
                {"CaveDwelling", StatNames.Cave},
                {"TemperatureTolerance", StatNames.TempTolerance},
                {"Flowering", StatNames.Pollination},
                {"Fertility", StatNames.Fertility},
                {"HumidityTolerance", StatNames.HumidTolerance},
                {"Speed", StatNames.Speed},
                {"Lifespan", StatNames.Lifespan},
                {"NeverSleeps", StatNames.Nocturnal}
            };

            SpeedMapping = new Dictionary<float, int>()
            {
                {0.3f, 1},
                {0.6f, 2},
                {0.8f, 3},
                {1f, 4},
                {1.2f, 5},
                {1.4f, 6},
                {1.7f, 7}
            };
            
            LifespanMapping = new Dictionary<int, int>()
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
            
            PollinationMapping = new Dictionary<int, int>()
            {
                {5, 1},
                {10, 2},
                {15, 3},
                {20, 4},
                {25, 5},
                {30, 6},
                {35, 7}
            };
            
            AdaptationMapping = new Dictionary<string, Adaptation>()
            {
                {"None", new Adaptation(0, 0)},
                {"Up 1", new Adaptation(1, 0)},
                {"Up 2", new Adaptation(2, 0)},
                {"Down 1", new Adaptation(0, 1)},
                {"Down 2", new Adaptation(0, 1)},
                {"Both 1", new Adaptation(1, 1)},
                {"Both 2", new Adaptation(2, 2)}
            };
            
            AreaMapping = new Dictionary<string, int>()
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
            
        public static BeeStack ToModelBee(this GameBeeModel raw)
        {
            var bee = new Bee
            {
                Generation = raw.Individual.Generation,
                Gender = raw.Name.Contains("drone")  ? Gender.Drone : Gender.Princess
            };
            var rawPrimary = raw.Individual.Active;
            var rawSecondary = raw.Individual.Inactive;
            Enum.TryParse(rawPrimary.Species, out Species speciePrimary);
            Enum.TryParse(rawSecondary.Species, out Species specieSecondary);
            Enum.TryParse(rawPrimary.Effect, out Effect effectPrimary);
            Enum.TryParse(rawSecondary.Effect, out Effect effectSecondary);
            Enum.TryParse(rawPrimary.FlowerProvider, out Flowers flowerPrimary);
            Enum.TryParse(rawSecondary.FlowerProvider, out Flowers flowerSecondary);
            
            var specieChromosome = new Chromosome<Species>()
            {
                Property = StatNames.Specie,
                Primary = new Gene<Species>() {Value = speciePrimary},
                Secondary = new Gene<Species>() {Value = specieSecondary}
            };
            
            var speedChromosome = new Chromosome<int>()
            {
                Property = StatNames.Speed,
                Primary = new Gene<int>() {Value = SpeedMapping[SpeedMapping.Keys.First(x => Math.Abs(x - rawPrimary.Speed) < 0.001)]},
                Secondary = new Gene<int>() {Value = SpeedMapping[SpeedMapping.Keys.First(x => Math.Abs(x - rawSecondary.Speed) < 0.001)]}
            };
            
            var pollinationChromosome = new Chromosome<int>()
            {
                Property = StatNames.Pollination,
                Primary = new Gene<int>() {Value = PollinationMapping[rawPrimary.Flowering]},
                Secondary = new Gene<int>() {Value = PollinationMapping[rawSecondary.Flowering]}
            };
            
            var areaChromosome = new Chromosome<int>()
            {
                Property = StatNames.Area,
                Primary = new Gene<int>() {Value = AreaMapping[rawPrimary.Territory]},
                Secondary = new Gene<int>() {Value = AreaMapping[rawSecondary.Territory]}
            };
            
            var lifespanChromosome = new Chromosome<int>()
            {
                Property = StatNames.Lifespan,
                Primary = new Gene<int>() {Value = LifespanMapping[rawPrimary.Lifespan]},
                Secondary = new Gene<int>() {Value = LifespanMapping[rawSecondary.Lifespan]}
            };
            
            var caveChromosome = new Chromosome<int>()
            {
                Property = StatNames.Cave,
                Primary = new Gene<int>() {Value = Convert.ToInt32(rawPrimary.CaveDwelling)},
                Secondary = new Gene<int>() {Value = Convert.ToInt32(rawSecondary.CaveDwelling)}
            };

            var nocturnalChromosome = new Chromosome<int>()
            {
                Property = StatNames.Nocturnal,
                Primary = new Gene<int>() {Value = Convert.ToInt32(rawPrimary.NeverSleeps)},
                Secondary = new Gene<int>() {Value = Convert.ToInt32(rawSecondary.NeverSleeps)}
            };
            
            var flyerChromosome = new Chromosome<int>()
            {
                Property = StatNames.Flyer,
                Primary = new Gene<int>() {Value = Convert.ToInt32(rawPrimary.ToleratesRain)},
                Secondary = new Gene<int>() {Value = Convert.ToInt32(rawSecondary.ToleratesRain)}
            };
            
            var diurnalChromosome = new Chromosome<int>()
            {
                Property = StatNames.Diurnal,
                Primary = new Gene<int>() {Value = 1},
                Secondary = new Gene<int>() {Value = 1}
            };
            
            var fertilityChromosome = new Chromosome<int>()
            {
                Property = StatNames.Fertility,
                Primary = new Gene<int>() {Value = rawPrimary.Fertility},
                Secondary = new Gene<int>() {Value = rawSecondary.Fertility}
            };
            
            var flowerChromosome = new Chromosome<Flowers>()
            {
                Property = StatNames.Flowers,
                Primary = new Gene<Flowers>() {Value = flowerPrimary},
                Secondary = new Gene<Flowers>() {Value = flowerSecondary}
            };
            
            var effectChromosome = new Chromosome<Effect>()
            {
                Property = StatNames.Effect,
                Primary = new Gene<Effect>() {Value = effectPrimary},
                Secondary = new Gene<Effect>() {Value = effectSecondary}
            };
            
            var humidityChromosome = new Chromosome<Adaptation>()
            {
                Property = StatNames.HumidTolerance,
                Primary = new Gene<Adaptation>() {Value = AdaptationMapping[rawPrimary.HumidityTolerance]},
                Secondary = new Gene<Adaptation>() {Value = AdaptationMapping[rawSecondary.HumidityTolerance]}
            };
            
            var temperatureChromosome = new Chromosome<Adaptation>()
            {
                Property = StatNames.TempTolerance,
                Primary = new Gene<Adaptation>() {Value = AdaptationMapping[rawPrimary.TemperatureTolerance]},
                Secondary = new Gene<Adaptation>() {Value = AdaptationMapping[rawSecondary.TemperatureTolerance]}
            };
            
            
            bee.Genotype.Chromosomes.Add(specieChromosome.Property, specieChromosome);
            bee.Genotype.Chromosomes.Add(lifespanChromosome.Property, lifespanChromosome);
            bee.Genotype.Chromosomes.Add(speedChromosome.Property, speedChromosome);
            bee.Genotype.Chromosomes.Add(pollinationChromosome.Property, pollinationChromosome);
            bee.Genotype.Chromosomes.Add(flowerChromosome.Property, flowerChromosome);
            bee.Genotype.Chromosomes.Add(fertilityChromosome.Property, fertilityChromosome);
            bee.Genotype.Chromosomes.Add(areaChromosome.Property, areaChromosome);
            bee.Genotype.Chromosomes.Add(temperatureChromosome.Property, temperatureChromosome);
            bee.Genotype.Chromosomes.Add(humidityChromosome.Property, humidityChromosome);
            bee.Genotype.Chromosomes.Add(diurnalChromosome.Property, diurnalChromosome);
            bee.Genotype.Chromosomes.Add(nocturnalChromosome.Property, nocturnalChromosome);
            bee.Genotype.Chromosomes.Add(flyerChromosome.Property, flyerChromosome);
            bee.Genotype.Chromosomes.Add(caveChromosome.Property, caveChromosome);
            bee.Genotype.Chromosomes.Add(effectChromosome.Property, effectChromosome);

            foreach (var gene in bee.Genotype.Chromosomes)
            {
                //TODO: Revision if possible to get this info from coming data
                //gene.Value.Primary.Dominant = BeeGeneticDatabase.GenesDominance[gene.Key][gene.Value.Primary.Value];
               //gene.Value.Secondary.Dominant = BeeGeneticDatabase.GenesDominance[gene.Key][gene.Value.Secondary.Value];
            }
            
            return new(bee, raw.Size);
        }
    }
}