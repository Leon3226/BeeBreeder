using System;
using System.Collections.Generic;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Common.Model.Bees
{
    public class Bee
    {
        public Gender Gender;
        public Genotype Genotype = new();
        public int Generation = 0;

        public IChromosome this[string index]
        {
            get => Genotype[index];
            set => Genotype[index] = value;
        }

        public List<Bee> Breed(Bee secondBee)
        {
            if (Gender == secondBee.Gender)
                return null;

            List<Bee> child = new List<Bee>();
            var princess = Gender == Gender.Drone ? secondBee : this;
            var fertility = ((IChromosome<int>)princess.Genotype[StatNames.Fertility]).ResultantAttribute;
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

        public bool CanLiveIn(Climate biome)
        {
            Climate specieClimate =
                BeeGeneticDatabase.SpeciesBiome[
                    (Species) Genotype[StatNames.Specie].ResultantAttribute];
            if (specieClimate == biome)
                return true;

            var humidityTolerance = (Adaptation)Genotype[StatNames.HumidTolerance].ResultantAttribute;
            var biomeHumidValue = (int) biome.Humidity;
            var beeHumidValue = (int) specieClimate.Humidity;
            if (biomeHumidValue > beeHumidValue + humidityTolerance.Up || biomeHumidValue < beeHumidValue - humidityTolerance.Down)
                return false;
            
            var temperatureTolerance = (Adaptation)Genotype[StatNames.TempTolerance].ResultantAttribute;
            var biomeTempValue = (int) biome.Temperature;
            var beeTempValue = (int) specieClimate.Temperature;
            if (biomeTempValue > beeTempValue + temperatureTolerance.Up || biomeTempValue < beeTempValue - temperatureTolerance.Down)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"{Gender} \n{Genotype}";
        }
    }
}