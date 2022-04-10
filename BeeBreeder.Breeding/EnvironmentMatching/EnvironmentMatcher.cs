using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.EnvironmentMatching
{
    public class EnvironmentMatcher : IEnvironmentMatcher
    {
        private readonly ISpecieClimateRepository _specieClimateRepository;
        private readonly IBiomeInfoRepository _biomeInfoRepository;

        public EnvironmentMatcher(ISpecieClimateRepository specieClimateRepository, IBiomeInfoRepository biomeInfoRepository)
        {
            _specieClimateRepository = specieClimateRepository;
            _biomeInfoRepository = biomeInfoRepository;
        }

        public (List<Temperature> temperatures, List<Humidity> humidities) AcceptableConditions(Bee bee)
        {
            Climate specieClimate =
                _specieClimateRepository.SpecieClimates[
                    (Species)bee.Genotype[Constants.StatNames.Specie].ResultantAttribute];

            var humidityTolerance = (Adaptation)bee.Genotype[Constants.StatNames.HumidTolerance].ResultantAttribute;
            var temperatureTolerance = (Adaptation)bee.Genotype[Constants.StatNames.TempTolerance].ResultantAttribute;

            var temperatures = Enum.GetValues(typeof(Temperature)).Cast<Temperature>().Where(x =>
                (int)x >= (int)specieClimate.Temperature - temperatureTolerance.Down &&
                (int)x <= (int)specieClimate.Temperature + temperatureTolerance.Up).ToList();
            var humidities = Enum.GetValues(typeof(Humidity)).Cast<Humidity>().Where(x =>
                (int)x >= (int)specieClimate.Humidity - humidityTolerance.Down &&
                (int)x <= (int)specieClimate.Humidity + humidityTolerance.Up).ToList();

            return (temperatures, humidities);
        }

        public List<Biome> AcceptableBiomes(Bee bee)
        {
            var conditions = AcceptableConditions(bee);
            return _biomeInfoRepository.BiomeClimates
                .Where(x => conditions.humidities.Contains(x.Value.Humidity) &&
                            conditions.temperatures.Contains(x.Value.Temperature)).Select(x => x.Key).ToList();
        }

        public bool CanLiveIn(Bee bee, Climate climate)
        {
            Climate specieClimate =
                _specieClimateRepository.SpecieClimates[
                    (Species)bee.Genotype[Constants.StatNames.Specie].ResultantAttribute];
            if (specieClimate == climate)
                return true;

            var humidityTolerance = (Adaptation)bee.Genotype[Constants.StatNames.HumidTolerance].ResultantAttribute;
            var biomeHumidValue = (int)climate.Humidity;
            var beeHumidValue = (int)specieClimate.Humidity;
            if (biomeHumidValue > beeHumidValue + humidityTolerance.Up ||
                biomeHumidValue < beeHumidValue - humidityTolerance.Down)
                return false;

            var temperatureTolerance = (Adaptation)bee.Genotype[Constants.StatNames.TempTolerance].ResultantAttribute;
            var biomeTempValue = (int)climate.Temperature;
            var beeTempValue = (int)specieClimate.Temperature;
            if (biomeTempValue > beeTempValue + temperatureTolerance.Up ||
                biomeTempValue < beeTempValue - temperatureTolerance.Down)
                return false;

            return true;
        }

        public bool CanLiveIn(Bee bee, Biome biome)
        {
            return CanLiveIn(bee, _biomeInfoRepository.BiomeClimates[biome]);
        }
    }
}
