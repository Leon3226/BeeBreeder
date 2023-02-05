using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public class StubBeeDataProvider :
        IGeneDominanceProvider,
        ISpecieCombinationsProvider,
        ISpecieStatsProvider,
        IBiomeInfoProvider,
        ISpecieClimateProvider,
        IBiomeClimateProvider
    {
        public List<SpecieCombination> SpecieCombinations { get; } = new();
        public Dictionary<string, Type> StatTypes { get; private set; } = new();
        public Dictionary<Biome, Climate> BiomeClimates { get; } = new();
        public Dictionary<string, Climate> SpecieClimates { get; } = new();
        public Dictionary<string, BeeInitialStats> SpecieStats { get; } = new();
        public Dictionary<string, int> DefaultSpeciePriorities { get; private set; } = new();
        public Dictionary<string, int> DefaultFlowersPriorities { get; private set; } = new();
        public Dictionary<string, int> DefaultEffectPriorities { get; private set; } = new();
        public Dictionary<string, Dictionary<object, bool>> GenesDominance { get; private set; }

        public StubBeeDataProvider()
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
            SpecieClimates.Add("Forest", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Meadows", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Common", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Cultivated", new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add("Noble", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Majestic", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Imperial", new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add("Diligent", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Unweary", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Industrious", new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add("Steadfast", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Valiant", new Climate(Temperature.Normal, Humidity.Normal));
            SpecieClimates.Add("Heroic", new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add("Sinister", new Climate(Temperature.Hellish, Humidity.Arid));
            SpecieClimates.Add("Fiendish", new Climate(Temperature.Hellish, Humidity.Arid));
            SpecieClimates.Add("Demonic", new Climate(Temperature.Hellish, Humidity.Arid));

            SpecieClimates.Add("Modest", new Climate(Temperature.Hot, Humidity.Arid));
            SpecieClimates.Add("Frugal", new Climate(Temperature.Hot, Humidity.Arid));
            SpecieClimates.Add("Austere", new Climate(Temperature.Hot, Humidity.Arid));

            SpecieClimates.Add("Ender", new Climate(Temperature.Cold, Humidity.Normal));

            SpecieClimates.Add("Tropical", new Climate(Temperature.Warm, Humidity.Damp));
            SpecieClimates.Add("Exotic", new Climate(Temperature.Warm, Humidity.Damp));
            SpecieClimates.Add("Edenic", new Climate(Temperature.Warm, Humidity.Damp));

            SpecieClimates.Add("Wintry", new Climate(Temperature.Icy, Humidity.Normal));
            SpecieClimates.Add("Icy", new Climate(Temperature.Icy, Humidity.Normal));
            SpecieClimates.Add("Glacial", new Climate(Temperature.Icy, Humidity.Normal));

            SpecieClimates.Add("Rural", new Climate(Temperature.Normal, Humidity.Normal));

            SpecieClimates.Add("Marshy", new Climate(Temperature.Normal, Humidity.Damp));
        }

        private void InitMutationTree()
        {
            var mundane = new List<string>()
            {
                "Forest", "Meadows", "Modest", "Tropical", "Wintry", "Marshy",
                "Valiant", "Ender"
            };

            mundane.ForEach(bee1 => mundane.ForEach(bee2 =>
            {
                if (bee1 != bee2)
                    SpecieCombinations.Add(new SpecieCombination(bee1, bee2, 0.15, "Common"));
            }));
            mundane.ForEach(bee =>
                SpecieCombinations.Add(new SpecieCombination("Common", bee, 0.12, "Cultivated")));

            SpecieCombinations.Add(new SpecieCombination("Common", "Cultivated", 0.10, "Noble"));
            SpecieCombinations.Add(new SpecieCombination("Noble", "Cultivated", 0.08, "Majestic"));
            SpecieCombinations.Add(new SpecieCombination("Majestic", "Noble", 0.08, "Imperial"));

            SpecieCombinations.Add(new SpecieCombination("Common", "Cultivated", 0.10, "Diligent"));
            SpecieCombinations.Add(new SpecieCombination("Diligent", "Cultivated", 0.08, "Unweary"));
            SpecieCombinations.Add(new SpecieCombination("Diligent", "Unweary", 0.08, "Industrious"));

            //TODO: Add Forest condition
            SpecieCombinations.Add(new SpecieCombination("Valiant", "Steadfast", 0.06, "Heroic"));

            //TODO: Add Hell condition
            SpecieCombinations.Add(new SpecieCombination("Modest", "Cultivated", 0.6, "Sinister"));
            SpecieCombinations.Add(new SpecieCombination("Tropical", "Cultivated", 0.6, "Sinister"));
            SpecieCombinations.Add(new SpecieCombination("Sinister", "Cultivated", 0.4, "Fiendish"));
            SpecieCombinations.Add(new SpecieCombination("Sinister", "Modest", 0.4, "Fiendish"));
            SpecieCombinations.Add(new SpecieCombination("Sinister", "Tropical", 0.4, "Fiendish"));
            SpecieCombinations.Add(new SpecieCombination("Sinister", "Fiendish", 0.25, "Demonic"));

            SpecieCombinations.Add(new SpecieCombination("Modest", "Sinister", 0.16, "Frugal"));
            SpecieCombinations.Add(new SpecieCombination("Modest", "Fiendish", 0.10, "Frugal"));
            SpecieCombinations.Add(new SpecieCombination("Modest", "Frugal", 0.8, "Austere"));

            SpecieCombinations.Add(new SpecieCombination("Austere", "Tropical", 0.12, "Exotic"));
            SpecieCombinations.Add(new SpecieCombination("Exotic", "Tropical", 0.08, "Edenic"));

            SpecieCombinations.Add(new SpecieCombination("Industrious", "Wintry", 0.12, "Icy"));
            SpecieCombinations.Add(new SpecieCombination("Icy", "Wintry", 0.08, "Glacial"));

            SpecieCombinations.Add(new SpecieCombination("Diligent", "Meadows", 0.12, "Rural"));
        }

        private void InitBeeStats()
        {
            #region Common

            SpecieStats.Add("Forest", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Forest"},
                    {Constants.StatNames.Lifespan, 2},
                    {Constants.StatNames.Speed, 1},
                    {Constants.StatNames.Pollination, 2},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 3},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Meadows", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Meadows"},
                    {Constants.StatNames.Lifespan, 2},
                    {Constants.StatNames.Speed, 1},
                    {Constants.StatNames.Pollination, 2},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Common", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Common"},
                    {Constants.StatNames.Lifespan, 2},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Cultivated", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Cultivated"},
                    {Constants.StatNames.Lifespan, 1},
                    {Constants.StatNames.Speed, 5},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            #endregion

            #region Noble

            SpecieStats.Add("Noble", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Noble"},
                    {Constants.StatNames.Lifespan, 3},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Majestic", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Majestic"},
                    {Constants.StatNames.Lifespan, 4},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 4},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Imperial", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Imperial"},
                    {Constants.StatNames.Lifespan, 4},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "Beatific"}
                }
            });

            #endregion

            #region Industrious

            SpecieStats.Add("Diligent", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Diligent"},
                    {Constants.StatNames.Lifespan, 3},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 3},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Unweary", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Unweary"},
                    {Constants.StatNames.Lifespan, 4},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Industrious", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Industrious"},
                    {Constants.StatNames.Lifespan, 4},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 5},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            #endregion

            #region Heroic

            SpecieStats.Add("Steadfast", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Steadfast"},
                    {Constants.StatNames.Lifespan, 5},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 1},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Valiant", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Valiant"},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 3},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 1},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Heroic", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Heroic"},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 3},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Flowers"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 1},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            #endregion

            #region Infernal

            SpecieStats.Add("Sinister", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Fiendish"},
                    {Constants.StatNames.Lifespan, 5},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 4},
                    {Constants.StatNames.Flowers, "Nether"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "Aggressive"}
                }
            });

            SpecieStats.Add("Fiendish", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Fiendish"},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 4},
                    {Constants.StatNames.Flowers, "Nether"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "Aggressive"}
                }
            });

            SpecieStats.Add("Demonic", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Fiendish"},
                    {Constants.StatNames.Lifespan, 8},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 4},
                    {Constants.StatNames.Flowers, "Nether"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "Aggressive"}
                }
            });

            #endregion

            #region Austere

            SpecieStats.Add("Modest", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Modest"},
                    {Constants.StatNames.Lifespan, 3},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Cacti"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Frugal", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Frugal"},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Cacti"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(1, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
                }
            });

            SpecieStats.Add("Austere", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Austere"},
                    {Constants.StatNames.Lifespan, 8},
                    {Constants.StatNames.Speed, 1},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Cacti"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(1, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "Creeper"}
                }
            });

            #endregion

            #region Ender

            SpecieStats.Add("Ender", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Ender"},
                    {Constants.StatNames.Lifespan, 6},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "End"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 2},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 1},
                    {Constants.StatNames.Flyer, 1},
                    {Constants.StatNames.Cave, 1},
                    {Constants.StatNames.Effect, "Ends"}
                }
            });

            #endregion

            #region Tropical

            SpecieStats.Add("Tropical", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Tropical"},
                    {Constants.StatNames.Lifespan, 3},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Jungle"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "Poison"}
                }
            });

            SpecieStats.Add("Exotic", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Exotic"},
                    {Constants.StatNames.Lifespan, 7},
                    {Constants.StatNames.Speed, 4},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Jungle"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(1, 1)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "Poison"}
                }
            });

            SpecieStats.Add("Edenic", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Edenic"},
                    {Constants.StatNames.Lifespan, 8},
                    {Constants.StatNames.Speed, 1},
                    {Constants.StatNames.Pollination, 1},
                    {Constants.StatNames.Flowers, "Jungle"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(2, 2)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "Explorer"}
                }
            });

            #endregion

            #region Agrarian

            SpecieStats.Add("Rural", new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {Constants.StatNames.Specie, "Rural"},
                    {Constants.StatNames.Lifespan, 2},
                    {Constants.StatNames.Speed, 2},
                    {Constants.StatNames.Pollination, 6},
                    {Constants.StatNames.Flowers, "Wheat"},
                    {Constants.StatNames.Fertility, 2},
                    {Constants.StatNames.Area, 1},
                    {Constants.StatNames.TempTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {Constants.StatNames.Diurnal, 1},
                    {Constants.StatNames.Nocturnal, 0},
                    {Constants.StatNames.Flyer, 0},
                    {Constants.StatNames.Cave, 0},
                    {Constants.StatNames.Effect, "None"}
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
                        {"Forest", true},
                        {"Meadows", true},
                        {"Common", true},
                        {"Cultivated", true},
                        {"Majestic", true},
                        {"Noble", false},
                        {"Imperial", false},
                        {"Diligent", false},
                        {"Unweary", true},
                        {"Industrious", false},
                        {"Steadfast", false},
                        {"Valiant", true},
                        {"Heroic", false},
                        {"Sinister", false},
                        {"Fiendish", true},
                        {"Demonic", false},
                        {"Frugal", true},
                        {"Modest", false},
                        {"Austere", false},
                        {"Ender", false},
                        {"Wintry", false},
                        {"Icy", true},
                        {"Glacial", false},
                        {"Marshy", true},
                        {"Tropical", false},
                        {"Exotic", true},
                        {"Edenic", false},
                        {"Rural", false},
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
                        {"Flowers", true},
                        {"Nether", false},
                        {"Cacti", false},
                        {"Jungle", false},
                        {"End", false},
                        {"Wheat", true},
                        {"Mushrooms", false}
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
                        {"None", true},
                        {"Beatific", false},
                        {"Creeper", true},
                        {"Aggressive", true},
                        {"Ends", true},
                        {"Explorer", false},
                        {"Poison", false}
                    }
                }
            };
        }

        private void InitDefaultSpeciePriorities()
        {
            DefaultSpeciePriorities = new Dictionary<string, int>()
            {
                {"Forest", 0},
                {"Meadows", 0},
                {"Common", 1},
                {"Cultivated", 2},
                {"Noble", 2},
                {"Majestic", 2},
                {"Imperial", 4},
                {"Diligent", 2},
                {"Unweary", 2},
                {"Industrious", 3},
                {"Steadfast", 1},
                {"Valiant", 1},
                {"Heroic", 2},
                {"Tropical", 0},
                {"Exotic", 2},
                {"Edenic", 3},
                {"Rural", 1},
                {"Austere", 1},
                {"Sinister",0},
                {"Fiendish",0},
                {"Demonic",0},
                {"Modest",0},
                {"Frugal",1},
                {"Ender",1},
                {"Wintry", 0},
                {"Icy",1},
                {"Glacial",2},
                {"Marshy", 0}
            };
        }

        private void InitDefaultEffectPriorities()
        {
            DefaultEffectPriorities = new Dictionary<string, int>()
            {
                {"None", 0},
                {"Poison", -3},
                {"Explorer", 2},
                {"Beatific", 3},
                {"Creeper", -10},
                {"Aggressive", -1},
                {"Ends", -2},
            };
        }

        private void InitDefaultFlowersPriorities()
        {
            DefaultFlowersPriorities = new Dictionary<string, int>()
            {
                {"Flowers", 3},
                {"Cacti", 0},
                {"End", -3},
                {"Jungle", 0},
                {"Mushrooms", -1},
                {"Nether", -2},
                {"Wheat", 2},
                {"Gourds", 0}
            };
        }

        private void InitStatTypes()
        {
            StatTypes = new Dictionary<string, Type>()
            {
                {Constants.StatNames.Specie, typeof(string)},
                {Constants.StatNames.Lifespan, typeof(int)},
                {Constants.StatNames.Speed, typeof(int)},
                {Constants.StatNames.Pollination, typeof(int)},
                {Constants.StatNames.Flowers, typeof(string)},
                {Constants.StatNames.Fertility, typeof(int)},
                {Constants.StatNames.Area, typeof(int)},
                {Constants.StatNames.TempTolerance, typeof(Adaptation)},
                {Constants.StatNames.HumidTolerance, typeof(Adaptation)},
                {Constants.StatNames.Diurnal, typeof(int)},
                {Constants.StatNames.Nocturnal, typeof(int)},
                {Constants.StatNames.Flyer, typeof(int)},
                {Constants.StatNames.Cave, typeof(int)},
                {Constants.StatNames.Effect, typeof(string)}
            };
        }

        #endregion

        public IEnumerable<SpecieCombination> GetPossibleMutations(string first, string second)
        {
            return SpecieCombinations.Where(x =>
                (x.Parent1 == first && x.Parent2 == second) ||
                (x.Parent2 == first && x.Parent1 == second));
        }

        //TODO: This shouldnt be here
        public (string, string) Mutations(Chromosome<string> first, Chromosome<string> second)
        {
            string GetMutation(string firstChromosome, string secondChromosome)
            {
                //TODO: Move random to another entity
                return GetPossibleMutations(firstChromosome, secondChromosome).FirstOrDefault(x => x.MutationChance > RandomGenerator.Double())?.MutationResult;
            }

            string GetRandomMutation()
            {
                return RandomGenerator.Double() > 0.5
                    ? GetMutation(first.Primary.Value, second.Secondary.Value)
                    : GetMutation(first.Secondary.Value, second.Primary.Value);
            }

            return (GetRandomMutation(), GetRandomMutation());
        }
    }
}
