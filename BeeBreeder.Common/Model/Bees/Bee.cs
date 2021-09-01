using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Common.Model.Bees
{
    public class Bee
    {
        public Gender Gender;
        public Genotype Genotype { get; set; } = new();
        public int Generation = 0;

        public IChromosome this[string index]
        {
            get => Genotype[index];
            set => Genotype[index] = value;
        }

        public SpecieChromosome SpecieChromosome => (SpecieChromosome) ChromosomeOf<Species>();

        public Chromosome<T> ChromosomeOf<T>(string property = null) where T : struct
        {
            return Genotype.Genes.FirstOrDefault(x =>
                    x.Value.Primary.Value.GetType() == typeof(T) && (property == null || property == x.Key))
                .Value as Chromosome<T>;
        }

        public List<Bee> Breed(Bee secondBee)
        {
            if (Gender == secondBee.Gender)
                return null;

            List<Bee> child = new List<Bee>();
            var princess = Gender == Gender.Drone ? secondBee : this;
            var fertility = ((IChromosome<int>) princess.Genotype[StatNames.Fertility]).ResultantAttribute;
            for (int i = 0; i < fertility + 1; i++)
            {
                child.Add(new Bee()
                    {
                        Gender = i == 0 ? Gender.Princess : Gender.Drone,
                        Genotype = Genotype.Cross(secondBee.Genotype),
                        Generation = princess.Generation + 1
                    }
                );
            }

            return child;
        }

        public bool CanLiveIn(Biome biome)
        {
            return CanLiveIn(BeeGeneticDatabase.Biomes[biome]);
        }

        public (List<Temperature> temperatures, List<Humidity> humidities) AcceptableConditions()
        {
            Climate specieClimate =
                BeeGeneticDatabase.SpeciesBiome[
                    (Species) Genotype[StatNames.Specie].ResultantAttribute];

            var humidityTolerance = (Adaptation) Genotype[StatNames.HumidTolerance].ResultantAttribute;
            var temperatureTolerance = (Adaptation) Genotype[StatNames.TempTolerance].ResultantAttribute;

            var temps = Enum.GetValues(typeof(Temperature)).Cast<Temperature>();
            var hums = Enum.GetValues(typeof(Humidity)).Cast<Humidity>();

            var temperatures = Enum.GetValues(typeof(Temperature)).Cast<Temperature>().Where(x =>
                (int) x >= (int) specieClimate.Temperature - temperatureTolerance.Down &&
                (int) x <= (int) specieClimate.Temperature + temperatureTolerance.Up).ToList();
            var humidities = Enum.GetValues(typeof(Humidity)).Cast<Humidity>().Where(x =>
                (int) x >= (int) specieClimate.Humidity - humidityTolerance.Down &&
                (int) x <= (int) specieClimate.Humidity + humidityTolerance.Up).ToList();

            return (temperatures, humidities);
        }

        public List<Biome> AcceptableBiomes()
        {
            var conditions = AcceptableConditions();
            return BeeGeneticDatabase.Biomes
                .Where(x => conditions.humidities.Contains(x.Value.Humidity) &&
                            conditions.temperatures.Contains(x.Value.Temperature)).Select(x => x.Key).ToList();
        }

        public bool CanLiveIn(Climate biome)
        {
            Climate specieClimate =
                BeeGeneticDatabase.SpeciesBiome[
                    (Species) Genotype[StatNames.Specie].ResultantAttribute];
            if (specieClimate == biome)
                return true;

            var humidityTolerance = (Adaptation) Genotype[StatNames.HumidTolerance].ResultantAttribute;
            var biomeHumidValue = (int) biome.Humidity;
            var beeHumidValue = (int) specieClimate.Humidity;
            if (biomeHumidValue > beeHumidValue + humidityTolerance.Up ||
                biomeHumidValue < beeHumidValue - humidityTolerance.Down)
                return false;

            var temperatureTolerance = (Adaptation) Genotype[StatNames.TempTolerance].ResultantAttribute;
            var biomeTempValue = (int) biome.Temperature;
            var beeTempValue = (int) specieClimate.Temperature;
            if (biomeTempValue > beeTempValue + temperatureTolerance.Up ||
                biomeTempValue < beeTempValue - temperatureTolerance.Down)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"{Gender} \n{Genotype}";
        }
    }
}