using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public class StubBeeDataRepository :
        IGeneDominanceRepository,
        ISpecieCombinationsRepository,
        ISpecieStatsRepository,
        IBiomeInfoRepository,
        ISpecieClimateRepository,
        IBiomeClimateRepository
    {
        public List<SpecieCombination> SpecieCombinations { get; private set; } = new();
        public Dictionary<string, Type> StatTypes { get; private set; } = new();
        public Dictionary<Biome, Climate> BiomeClimates { get; private set; } = new();
        public Dictionary<Species, Climate> SpecieClimates { get; private set; } = new();
        public Dictionary<Species, BeeInitialStats> SpecieStats { get; private set; } = new();
        public Dictionary<Species, int> DefaultSpeciePriorities { get; private set; } = new();
        public Dictionary<Flowers, int> DefaultFlowersPriorities { get; private set; } = new();
        public Dictionary<Effect, int> DefaultEffectPriorities { get; private set; } = new();
        public Dictionary<string, Dictionary<object, bool>> GenesDominance { get; private set; }

        public StubBeeDataRepository()
        {
            //TODO: Move all of this to the configs
            InitMutationTree();
            InitStatTypes();
            InitBeeStats();
            InitBiomes();
            InitSpeciesPreferences();
            InitStatDominanceTree();
            InitDefaultSpeciePriorities();
            InitDefaultEffectPriorities();
            InitDefaultFlowersPriorities();
        }

        #region Inits

        private void InitBiomes()
        {
            BiomeClimates.Add(Biome.Forest, new Climate(Temperature.Normal, Humidity.Normal));
            BiomeClimates.Add(Biome.Meadow, new Climate(Temperature.Normal, Humidity.Normal));
            BiomeClimates.Add(Biome.Plains, new Climate(Temperature.Normal, Humidity.Normal));
            BiomeClimates.Add(Biome.Desert, new Climate(Temperature.Hot, Humidity.Arid));
            BiomeClimates.Add(Biome.Jungle, new Climate(Temperature.Warm, Humidity.Damp));
            BiomeClimates.Add(Biome.Tundra, new Climate(Temperature.Icy, Humidity.Arid));
            BiomeClimates.Add(Biome.SnowForest, new Climate(Temperature.Cold, Humidity.Normal));
        }

        private void InitSpeciesPreferences()
        {
            SpecieClimates.Add(Species.Forest, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Meadows, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Common, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Cultivated, new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add(Species.Noble, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Majestic, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Imperial, new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add(Species.Diligent, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Unweary, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Industrious, new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add(Species.Steadfast, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Valiant, new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add(Species.Heroic, new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add(Species.Sinister, new Climate(Temperature.Hellish, Humidity.Arid));
            SpecieClimates.Add(Species.Fiendish, new Climate(Temperature.Hellish, Humidity.Arid));
            SpecieClimates.Add(Species.Demonic, new Climate(Temperature.Hellish, Humidity.Arid));

            SpecieClimates.Add(Species.Modest, new Climate(Temperature.Hot, Humidity.Arid));
            SpecieClimates.Add(Species.Frugal, new Climate(Temperature.Hot, Humidity.Arid));
            SpecieClimates.Add(Species.Austere, new Climate(Temperature.Hot, Humidity.Arid));

            SpecieClimates.Add(Species.Ender, new Climate(Temperature.Cold, Humidity.Normal));

            SpecieClimates.Add(Species.Tropical, new Climate(Temperature.Warm, Humidity.Damp));
            SpecieClimates.Add(Species.Exotic, new Climate(Temperature.Warm, Humidity.Damp));
            SpecieClimates.Add(Species.Edenic, new Climate(Temperature.Warm, Humidity.Damp));

            SpecieClimates.Add(Species.Wintry, new Climate(Temperature.Icy, Humidity.Normal));
            SpecieClimates.Add(Species.Icy, new Climate(Temperature.Icy, Humidity.Normal));
            SpecieClimates.Add(Species.Glacial, new Climate(Temperature.Icy, Humidity.Normal));

            SpecieClimates.Add(Species.Rural, new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add(Species.Marshy, new Climate(Temperature.Normal, Humidity.Damp));
        }

        private void InitMutationTree()
        {
            var mundane = new List<Species>()
            {
                Species.Forest, Species.Meadows, Species.Modest, Species.Tropical, Species.Wintry, Species.Marshy,
                Species.Valiant, Species.Ender
            };

            mundane.ForEach(bee1 => mundane.ForEach(bee2 =>
            {
                if (bee1 != bee2)
                    SpecieCombinations.Add(new SpecieCombination(bee1, bee2, 0.15, Species.Common));
            }));
            mundane.ForEach(bee =>
                SpecieCombinations.Add(new SpecieCombination(Species.Common, bee, 0.12, Species.Cultivated)));

            SpecieCombinations.Add(new SpecieCombination(Species.Common, Species.Cultivated, 0.10, Species.Noble));
            SpecieCombinations.Add(new SpecieCombination(Species.Noble, Species.Cultivated, 0.08, Species.Majestic));
            SpecieCombinations.Add(new SpecieCombination(Species.Majestic, Species.Noble, 0.08, Species.Imperial));

            SpecieCombinations.Add(new SpecieCombination(Species.Common, Species.Cultivated, 0.10, Species.Diligent));
            SpecieCombinations.Add(new SpecieCombination(Species.Diligent, Species.Cultivated, 0.08, Species.Unweary));
            SpecieCombinations.Add(new SpecieCombination(Species.Diligent, Species.Unweary, 0.08, Species.Industrious));

            //TODO: Add Forest condition
            SpecieCombinations.Add(new SpecieCombination(Species.Valiant, Species.Steadfast, 0.06, Species.Heroic));

            //TODO: Add Hell condition
            SpecieCombinations.Add(new SpecieCombination(Species.Modest, Species.Cultivated, 0.6, Species.Sinister));
            SpecieCombinations.Add(new SpecieCombination(Species.Tropical, Species.Cultivated, 0.6, Species.Sinister));
            SpecieCombinations.Add(new SpecieCombination(Species.Sinister, Species.Cultivated, 0.4, Species.Fiendish));
            SpecieCombinations.Add(new SpecieCombination(Species.Sinister, Species.Modest, 0.4, Species.Fiendish));
            SpecieCombinations.Add(new SpecieCombination(Species.Sinister, Species.Tropical, 0.4, Species.Fiendish));
            SpecieCombinations.Add(new SpecieCombination(Species.Sinister, Species.Fiendish, 0.25, Species.Demonic));

            SpecieCombinations.Add(new SpecieCombination(Species.Modest, Species.Sinister, 0.16, Species.Frugal));
            SpecieCombinations.Add(new SpecieCombination(Species.Modest, Species.Fiendish, 0.10, Species.Frugal));
            SpecieCombinations.Add(new SpecieCombination(Species.Modest, Species.Frugal, 0.8, Species.Austere));

            SpecieCombinations.Add(new SpecieCombination(Species.Austere, Species.Tropical, 0.12, Species.Exotic));
            SpecieCombinations.Add(new SpecieCombination(Species.Exotic, Species.Tropical, 0.08, Species.Edenic));

            SpecieCombinations.Add(new SpecieCombination(Species.Industrious, Species.Wintry, 0.12, Species.Icy));
            SpecieCombinations.Add(new SpecieCombination(Species.Icy, Species.Wintry, 0.08, Species.Glacial));

            SpecieCombinations.Add(new SpecieCombination(Species.Diligent, Species.Meadows, 0.12, Species.Rural));
        }

        private void InitBeeStats()
        {
            #region Common

            SpecieStats.Add(Species.Forest, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Forest},
                    {Constants.StatNames.Lifespan, 2},
                    {Constants.StatNames.Speed, 1},
                    {Constants.StatNames.Pollination, 2},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 3},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Meadows, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Meadows},
                    {Constants.StatNames.Lifespan, 2},
                    {Constants.StatNames.Speed, 1},
                    {Constants.StatNames.Pollination, 2},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Common, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Common},
                    {Constants.StatNames.Lifespan, 2},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Cultivated, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Cultivated},
                    {Constants.StatNames.Lifespan, 1},
                    {Constants.StatNames.Speed, 5},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            #endregion

            #region Noble

            SpecieStats.Add(Species.Noble, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Noble},
                    {Constants.StatNames.Lifespan, 3},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Majestic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Majestic},
                    {Constants.StatNames.Lifespan, 4},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 4},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Imperial, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Imperial},
                    {Constants.StatNames.Lifespan, 4},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.Beatific}
                }
            });

            #endregion

            #region Industrious

            SpecieStats.Add(Species.Diligent, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Diligent},
                    {Constants.StatNames.Lifespan, 3},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 3},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Unweary, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Unweary},
                    {Constants.StatNames.Lifespan, 4},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Industrious, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Industrious},
                    {Constants.StatNames.Lifespan, 4},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 5},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            #endregion

            #region Heroic

            SpecieStats.Add(Species.Steadfast, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Steadfast},
                    {Constants.StatNames.Lifespan, 5},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 1},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Valiant, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Valiant},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 3},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 1},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Heroic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Heroic},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 3},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Flowers},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 1},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            #endregion

            #region Infernal

            SpecieStats.Add(Species.Sinister, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Fiendish},
                    {Constants.StatNames.Lifespan, 5},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 4},
                    {Constants.StatNames.Flowers, Flowers.Nether},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.Aggressive}
                }
            });

            SpecieStats.Add(Species.Fiendish, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Fiendish},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 4},
                    {Constants.StatNames.Flowers, Flowers.Nether},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.Aggressive}
                }
            });

            SpecieStats.Add(Species.Demonic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Fiendish},
                    {Constants.StatNames.Lifespan, 8},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 4},
                    {Constants.StatNames.Flowers, Flowers.Nether},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.Aggressive}
                }
            });

            #endregion

            #region Austere

            SpecieStats.Add(Species.Modest, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Modest},
                    {Constants.StatNames.Lifespan, 3},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Cacti},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Frugal, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Frugal},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Cacti},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(1, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            SpecieStats.Add(Species.Austere, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Austere},
                    {Constants.StatNames.Lifespan, 8},
                    {Constants.StatNames.Speed, 1},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Cacti},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(1, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.Creeper}
                }
            });

            #endregion

            #region Ender

            SpecieStats.Add(Species.Ender, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Ender},
                    {Constants.StatNames.Lifespan, 6},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.End},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 2},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 1},
                    {Constants.StatNames.Cave, 1},
                    {Constants.StatNames.Effect, Effect.Ends}
                }
            });

            #endregion

            #region Tropical

            SpecieStats.Add(Species.Tropical, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Tropical},
                    {Constants.StatNames.Lifespan, 3},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Jungle},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.Poison}
                }
            });

            SpecieStats.Add(Species.Exotic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Exotic},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Jungle},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.Poison}
                }
            });

            SpecieStats.Add(Species.Edenic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Edenic},
                    {Constants.StatNames.Lifespan, 8},
                    {Constants.StatNames.Speed, 1},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, Flowers.Jungle},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(2, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.Explorer}
                }
            });

            #endregion

            #region Agrarian

            SpecieStats.Add(Species.Rural, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, Species.Rural},
                    {Constants.StatNames.Lifespan, 2},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 6},
                    {Constants.StatNames.Flowers, Flowers.Wheat},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, Effect.None}
                }
            });

            #endregion
        }

        private void InitStatDominanceTree()
        {
            GenesDominance = new Dictionary<string, Dictionary<object, bool>>
            {
                {
                    Constants.StatNames.Specie, new Dictionary<object, bool>()
                    {
                        {Species.Forest, true},
                        {Species.Meadows, true},
                        {Species.Common, true},
                        {Species.Cultivated, true},
                        {Species.Majestic, true},
                        {Species.Noble, false},
                        {Species.Imperial, false},
                        {Species.Diligent, false},
                        {Species.Unweary, true},
                        {Species.Industrious, false},
                        {Species.Steadfast, false},
                        {Species.Valiant, true},
                        {Species.Heroic, false},
                        {Species.Sinister, false},
                        {Species.Fiendish, true},
                        {Species.Demonic, false},
                        {Species.Frugal, true},
                        {Species.Modest, false},
                        {Species.Austere, false},
                        {Species.Ender, false},
                        {Species.Wintry, false},
                        {Species.Icy, true},
                        {Species.Glacial, false},
                        {Species.Marshy, true},
                        {Species.Tropical, false},
                        {Species.Exotic, true},
                        {Species.Edenic, false},
                        {Species.Rural, false},
                    }
                },
                {
                    Constants.StatNames.Lifespan, new Dictionary<object, bool>()
                    {
                        {1, false},
                        {2, true},
                        {3, true},
                        {4, true},
                        {5, false},
                        {6, true},
                        {7, false},
                        {8, false},
                        {9, false}
                    }
                },
                {
                    Constants.StatNames.Speed, new Dictionary<object, bool>()
                    {
                        {1, true},
                        {2, true},
                        {3, true},
                        {4, false},
                        {5, true},
                        {6, false},
                        {7, false}
                    }
                },
                {
                    Constants.StatNames.Pollination, new Dictionary<object, bool>()
                    {
                        {1, true},
                        {2, false},
                        {3, false},
                        {4, false},
                        {5, false},
                        {6, false},
                        {7, false}
                    }
                },
                {
                    Constants.StatNames.Flowers, new Dictionary<object, bool>()
                    {
                        {Flowers.Flowers, true},
                        {Flowers.Nether, false},
                        {Flowers.Cacti, false},
                        {Flowers.Jungle, false},
                        {Flowers.End, false},
                        {Flowers.Wheat, true},
                        {Flowers.Mushrooms, false}
                    }
                },
                {
                    Constants.StatNames.Fertility, new Dictionary<object, bool>()
                    {
                        {1, true},
                        {2, true},
                        {3, false},
                        {4, false},
                    }
                },
                {
                    Constants.StatNames.Area, new Dictionary<object, bool>()
                    {
                        {1, false},
                        {2, false},
                        {3, false},
                        {4, false},
                    }
                },
                {
                    Constants.StatNames.TempTolerance, new Dictionary<object, bool>()
                    {
                        {new Adaptation(0, 0), false},
                        {new Adaptation(1, 1), true},
                        {new Adaptation(2, 2), false},
                        {new Adaptation(1, 0), true},
                        {new Adaptation(2, 0), false},
                        {new Adaptation(0, 1), true},
                        {new Adaptation(0, 2), false},
                    }
                },
                {
                    Constants.StatNames.HumidTolerance, new Dictionary<object, bool>()
                    {
                        {new Adaptation(0, 0), false},
                        {new Adaptation(1, 1), true},
                        {new Adaptation(2, 2), false},
                        {new Adaptation(1, 0), true},
                        {new Adaptation(2, 0), false},
                        {new Adaptation(0, 1), true},
                        {new Adaptation(0, 2), false},
                    }
                },
                {
                    Constants.StatNames.Nocturnal, new Dictionary<object, bool>()
                    {
                        {0, false},
                        {1, false}
                    }
                },
                {
                    Constants.StatNames.Diurnal, new Dictionary<object, bool>()
                    {
                        {0, false},
                        {1, false}
                    }
                },
                {
                    Constants.StatNames.Flyer, new Dictionary<object, bool>()
                    {
                        {0, false},
                        {1, false}
                    }
                },
                {
                    Constants.StatNames.Cave, new Dictionary<object, bool>()
                    {
                        {0, false},
                        {1, false}
                    }
                },
                {
                    Constants.StatNames.Effect, new Dictionary<object, bool>()
                    {
                        {Effect.None, true},
                        {Effect.Beatific, false},
                        {Effect.Creeper, true},
                        {Effect.Aggressive, true},
                        {Effect.Ends, true},
                        {Effect.Explorer, false},
                        {Effect.Poison, false}
                    }
                }
            };
        }

        private void InitDefaultSpeciePriorities()
        {
            DefaultSpeciePriorities = new Dictionary<Species, int>()
            {
                {Species.Forest, 0},
                {Species.Meadows, 0},
                {Species.Common, 1},
                {Species.Cultivated, 2},
                {Species.Noble, 2},
                {Species.Majestic, 2},
                {Species.Imperial, 4},
                {Species.Diligent, 2},
                {Species.Unweary, 2},
                {Species.Industrious, 3},
                {Species.Steadfast, 1},
                {Species.Valiant, 1},
                {Species.Heroic, 2},
                {Species.Tropical, 0},
                {Species.Exotic, 2},
                {Species.Edenic, 3},
                {Species.Rural, 1},
                {Species.Austere, 1},
                {Species.Sinister,0},
                {Species.Fiendish,0},
                {Species.Demonic,0},
                {Species.Modest,0},
                {Species.Frugal,1},
                {Species.Ender,1},
                {Species.Wintry, 0},
                {Species.Icy,1},
                {Species.Glacial,2},
                {Species.Marshy, 0}
            };
        }

        private void InitDefaultEffectPriorities()
        {
            DefaultEffectPriorities = new Dictionary<Effect, int>()
            {
                {Effect.None, 0},
                {Effect.Poison, -3},
                {Effect.Explorer, 2},
                {Effect.Beatific, 3},
                {Effect.Creeper, -10},
                {Effect.Aggressive, -1},
                {Effect.Ends, -2},
            };
        }

        private void InitDefaultFlowersPriorities()
        {
            DefaultFlowersPriorities = new Dictionary<Flowers, int>()
            {
                {Flowers.Flowers, 3},
                {Flowers.Cacti, 0},
                {Flowers.End, -3},
                {Flowers.Jungle, 0},
                {Flowers.Mushrooms, -1},
                {Flowers.Nether, -2},
                {Flowers.Wheat, 2},
                {Flowers.Gourds, 0}
            };
        }

        private void InitStatTypes()
        {
            StatTypes = new Dictionary<string, Type>()
            {
                {Constants.StatNames.Specie, typeof(Species)},
                {Constants.StatNames.Lifespan, typeof(int)},
                {Constants.StatNames.Speed, typeof(int)},
                {Constants.StatNames.Pollination, typeof(int)},
                {Constants.StatNames.Flowers, typeof(Flowers)},
                {Constants.StatNames.Fertility, typeof(int)},
                {Constants.StatNames.Area, typeof(int)},
                {Constants.StatNames.TempTolerance, typeof(Adaptation)},
                {Constants.StatNames.HumidTolerance, typeof(Adaptation)},
                {Constants.StatNames.Diurnal, typeof(int)},
                {Constants.StatNames.Nocturnal, typeof(int)},
                {Constants.StatNames.Flyer, typeof(int)},
                {Constants.StatNames.Cave, typeof(int)},
                {Constants.StatNames.Effect, typeof(Effect)}
            };
        }

        #endregion

        public IEnumerable<SpecieCombination> GetPossibleMutations(Species first, Species second)
        {
            return SpecieCombinations.Where(x =>
                (x.Parent1 == first && x.Parent2 == second) ||
                (x.Parent2 == first && x.Parent1 == second));
        }

        public (Species?, Species?) Mutations(Chromosome<Species> first, Chromosome<Species> second)
        {
            Species? GetMutation(Species firstChromosome, Species secondChromosome)
            {
                //TODO: Move random to another entity
                return GetPossibleMutations(firstChromosome, secondChromosome).FirstOrDefault(x => x.MutationChance > RandomGenerator.Double())?.MutationResult;
            }

            Species? GetRandomMutation()
            {
                return RandomGenerator.Double() > 0.5
                    ? GetMutation(first.Primary.Value, second.Secondary.Value)
                    : GetMutation(first.Secondary.Value, second.Primary.Value);
            }

            return (GetRandomMutation(), GetRandomMutation());
        }
    }
}
