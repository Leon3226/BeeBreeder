using System.Collections.Generic;

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
        }
        
        public static List<SpecieCombination> SpecieCombinations = new();
        public static Dictionary<Species, BeeInitialStats> SpecieStats = new();
        public static Dictionary<string, Dictionary<object, bool>> GenesDominancies;

        static BeeGeneticDatabase()
        {
            //TODO: Move all of this to the configs
            InitMutationTree();
            InitBeeStats();
            InitStatDominancyTree();
            
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
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
                    {StatNames.Area, 1}
                }
            });
        }
        private static void InitStatDominancyTree()
        {
            GenesDominancies = new Dictionary<string, Dictionary<object, bool>>
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
                }
            };
        }
    }
}