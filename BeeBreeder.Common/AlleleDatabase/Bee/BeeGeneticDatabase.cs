using System;
using System.Collections.Generic;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Common.AlleleDatabase.Bee
{
    public static class BeeGeneticDatabase
    {
        public static class StatNames
        {
            public const string Specie = "Specie";
            public const string Lifespan = "Lifespan";
            public const string Speed = "Speed";
            public const string Pollination = "Pollination";
            public const string Flowers = "Flowers";
            public const string Fertility = "Fertility";
            public const string Area = "Area";
            public const string TempTolerance = "Temperature Tolerance";
            public const string HumidTolerance = "Humidity Tolerance";
            public const string Diurnal  = "Diurnal";
            public const string Nocturnal = "Nocturnal";
            public const string Flyer = "Flyer";
            public const string Cave = "Cave";
            public const string Effect = "Effect";

        }
        
        public static readonly List<SpecieCombination> SpecieCombinations = new();
        public static Dictionary<string, Type> StatTypes = new();
        public static Dictionary<Biome, Climate> Biomes = new();
        public static Dictionary<Species, Climate> SpeciesBiome = new();
        public static readonly Dictionary<Species, BeeInitialStats> SpecieStats = new();
        public static Dictionary<Species, int> DefaultSpeciePriorities = new();
        public static Dictionary<Flowers, int> DefaultFlowersPriorities = new();
        public static Dictionary<Effect, int> DefaultEffectPriorities = new();
        public static Dictionary<string, Dictionary<object, bool>> GenesDominance;

        static BeeGeneticDatabase()
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

        private static void InitBiomes()
        {
            Biomes.Add(Biome.Forest, new Climate(Temperature.Normal, Humidity.Normal));
            Biomes.Add(Biome.Meadow, new Climate(Temperature.Normal, Humidity.Normal));
            Biomes.Add(Biome.Plains, new Climate(Temperature.Normal, Humidity.Normal));
            Biomes.Add(Biome.Desert, new Climate(Temperature.Hot, Humidity.Arid));
            Biomes.Add(Biome.Jungle, new Climate(Temperature.Warm, Humidity.Damp));
            Biomes.Add(Biome.Tundra, new Climate(Temperature.Icy, Humidity.Arid));
            Biomes.Add(Biome.SnowForest, new Climate(Temperature.Cold, Humidity.Normal));
        }
        private static void InitSpeciesPreferences()
        {
            SpeciesBiome.Add(Species.Forest, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Meadows, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Common, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Noble, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Majestic, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Imperial, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Unweary, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Valiant, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Cultivated, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Diligent, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Rural, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Steadfast, new Climate(Temperature.Normal, Humidity.Normal));
            SpeciesBiome.Add(Species.Edenic, new Climate(Temperature.Warm, Humidity.Damp));
            SpeciesBiome.Add(Species.Tropical, new Climate(Temperature.Warm, Humidity.Damp));
            SpeciesBiome.Add(Species.Exotic, new Climate(Temperature.Warm, Humidity.Damp));
            SpeciesBiome.Add(Species.Austere, new Climate(Temperature.Hot, Humidity.Arid));
        }
        private static void InitMutationTree()
        {
            SpecieCombinations.Add(new SpecieCombination(Species.Forest, Species.Meadows, 0.15, Species.Common));
            SpecieCombinations.Add(new SpecieCombination(Species.Common, Species.Meadows, 0.12, Species.Cultivated));
            SpecieCombinations.Add(new SpecieCombination(Species.Common, Species.Forest, 0.12, Species.Cultivated));
            SpecieCombinations.Add(new SpecieCombination(Species.Common, Species.Cultivated, 0.10, Species.Noble));
            SpecieCombinations.Add(new SpecieCombination(Species.Noble, Species.Cultivated, 0.08, Species.Majestic));
            SpecieCombinations.Add(new SpecieCombination(Species.Majestic, Species.Noble, 0.08, Species.Imperial));
            SpecieCombinations.Add(new SpecieCombination(Species.Common, Species.Cultivated, 0.10, Species.Diligent));
            SpecieCombinations.Add(new SpecieCombination(Species.Diligent, Species.Cultivated, 0.08, Species.Unweary));
            SpecieCombinations.Add(new SpecieCombination(Species.Diligent, Species.Unweary, 0.08, Species.Industrious));
            SpecieCombinations.Add(new SpecieCombination(Species.Valiant, Species.Steadfast, 0.06, Species.Heroic));
            SpecieCombinations.Add(new SpecieCombination(Species.Austere, Species.Tropical, 0.12, Species.Exotic));
            SpecieCombinations.Add(new SpecieCombination(Species.Exotic, Species.Tropical, 0.08, Species.Edenic));
            SpecieCombinations.Add(new SpecieCombination(Species.Diligent, Species.Meadows, 0.12, Species.Rural));
        }   
        private static void InitBeeStats()
        {
            SpecieStats.Add(Species.Forest, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Forest},
                    {StatNames.Lifespan, 2},
                    {StatNames.Speed, 1},
                    {StatNames.Pollination, 2},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 3},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Meadows, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Meadows},
                    {StatNames.Lifespan, 2},
                    {StatNames.Speed, 1},
                    {StatNames.Pollination, 2},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Common, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Common},
                    {StatNames.Lifespan, 2},
                    {StatNames.Speed, 2},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Cultivated, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Cultivated},
                    {StatNames.Lifespan, 1},
                    {StatNames.Speed, 5},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Noble, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Noble},
                    {StatNames.Lifespan, 3},
                    {StatNames.Speed, 5},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Majestic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Majestic},
                    {StatNames.Lifespan, 4},
                    {StatNames.Speed, 4},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 4},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Imperial, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Imperial},
                    {StatNames.Lifespan, 4},
                    {StatNames.Speed, 2},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.Beatific}
                }
            });
            
            SpecieStats.Add(Species.Diligent, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Diligent},
                    {StatNames.Lifespan, 3},
                    {StatNames.Speed, 2},
                    {StatNames.Pollination, 3},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Unweary, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Unweary},
                    {StatNames.Lifespan, 4},
                    {StatNames.Speed, 4},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Industrious, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Industrious},
                    {StatNames.Lifespan, 4},
                    {StatNames.Speed, 2},
                    {StatNames.Pollination, 5},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Steadfast, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Steadfast},
                    {StatNames.Lifespan, 5},
                    {StatNames.Speed, 2},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 1},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 1},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Valiant, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Valiant},
                    {StatNames.Lifespan, 7},
                    {StatNames.Speed, 3},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 1},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 1},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Heroic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Heroic},
                    {StatNames.Lifespan, 7},
                    {StatNames.Speed, 3},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Flowers},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 1},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 1},
                    {StatNames.Effect, Effect.None}
                }
            });
            
            SpecieStats.Add(Species.Tropical, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Tropical},
                    {StatNames.Lifespan, 3},
                    {StatNames.Speed, 2},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Jungle},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(1, 1)},
                    {StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.Poison}
                }
            });
            
            SpecieStats.Add(Species.Exotic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Exotic},
                    {StatNames.Lifespan, 7},
                    {StatNames.Speed, 4},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Jungle},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(1, 1)},
                    {StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.Poison}
                }
            });
            
            SpecieStats.Add(Species.Edenic, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Edenic},
                    {StatNames.Lifespan, 8},
                    {StatNames.Speed, 1},
                    {StatNames.Pollination, 1},
                    {StatNames.Flowers, Flowers.Jungle},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(2, 2)},
                    {StatNames.HumidTolerance, new Adaptation(0, 1)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.Explorer}
                }
            });
            
            SpecieStats.Add(Species.Rural, new BeeInitialStats()
            {
                Characteristics = new Dictionary<string, object>()
                {
                    {StatNames.Specie, Species.Rural},
                    {StatNames.Lifespan, 2},
                    {StatNames.Speed, 2},
                    {StatNames.Pollination, 6},
                    {StatNames.Flowers, Flowers.Wheat},
                    {StatNames.Fertility, 2},
                    {StatNames.Area, 1},
                    {StatNames.TempTolerance, new Adaptation(0, 0)},
                    {StatNames.HumidTolerance, new Adaptation(0, 0)},
                    {StatNames.Diurnal, 1},
                    {StatNames.Nocturnal, 0},
                    {StatNames.Flyer, 0},
                    {StatNames.Cave, 0},
                    {StatNames.Effect, Effect.None}
                }
            });
        }
        private static void InitStatDominanceTree()
        {
            GenesDominance = new Dictionary<string, Dictionary<object, bool>>
            {
                {
                    StatNames.Specie, new Dictionary<object, bool>()
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
                        {Species.Tropical, false},
                        {Species.Exotic, true},
                        {Species.Edenic, false},
                        {Species.Rural,false},
                        {Species.Austere, false}
                    }
                },
                {
                    StatNames.Lifespan, new Dictionary<object, bool>()
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
                    StatNames.Speed, new Dictionary<object, bool>()
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
                    StatNames.Pollination, new Dictionary<object, bool>()
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
                    StatNames.Flowers, new Dictionary<object, bool>()
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
                    StatNames.Fertility, new Dictionary<object, bool>()
                    {
                        {1, true},
                        {2, true},
                        {3, false},
                        {4, false},
                    }
                },
                {
                    StatNames.Area, new Dictionary<object, bool>()
                    {
                        {1, false},
                        {2, false},
                        {3, false},
                        {4, false},
                    }
                },
                {
                    StatNames.TempTolerance, new Dictionary<object, bool>()
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
                    StatNames.HumidTolerance, new Dictionary<object, bool>()
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
                    StatNames.Nocturnal, new Dictionary<object, bool>()
                    {
                        {0, false},
                        {1, false}
                    }
                },
                {
                    StatNames.Diurnal, new Dictionary<object, bool>()
                    {
                        {0, false},
                        {1, false}
                    }
                },
                {
                    StatNames.Flyer, new Dictionary<object, bool>()
                    {
                        {0, false},
                        {1, false}
                    }
                },
                {
                    StatNames.Cave, new Dictionary<object, bool>()
                    {
                        {0, false},
                        {1, false}
                    }
                },
                {
                    StatNames.Effect, new Dictionary<object, bool>()
                    {
                        {Effect.None, true},
                        {Effect.Beatific, false},
                        {Effect.Explorer, false},
                        {Effect.Poison, false}
                    }
                }
                
            };
        }
        private static void InitDefaultSpeciePriorities()
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
                {Species.Steadfast,1},
                {Species.Valiant,1},
                {Species.Heroic,2},
                {Species.Tropical,0},
                {Species.Exotic,2},
                {Species.Edenic,3},
                {Species.Rural,1},
                {Species.Austere,1}
            };
        }
        private static void InitDefaultEffectPriorities()
        {
            DefaultEffectPriorities = new Dictionary<Effect, int>()
            {
                {Effect.None, 0},
                {Effect.Poison, -1},
                {Effect.Explorer, 2},
                {Effect.Beatific, 2}
            };
        }
        private static void InitDefaultFlowersPriorities()
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
        private static void InitStatTypes()
        {
            StatTypes = new Dictionary<string, Type>()
            {
                {StatNames.Specie, typeof(Species)},
                {StatNames.Lifespan, typeof(int)},
                {StatNames.Speed, typeof(int)},
                {StatNames.Pollination, typeof(int)},
                {StatNames.Flowers, typeof(Flowers)},
                {StatNames.Fertility, typeof(int)},
                {StatNames.Area, typeof(int)},
                {StatNames.TempTolerance, typeof(Adaptation)},
                {StatNames.HumidTolerance, typeof(Adaptation)},
                {StatNames.Diurnal, typeof(int)},
                {StatNames.Nocturnal, typeof(int)},
                {StatNames.Flyer, typeof(int)},
                {StatNames.Cave, typeof(int)},
                {StatNames.Effect, typeof(Effect)}
            };
        }
    }
}