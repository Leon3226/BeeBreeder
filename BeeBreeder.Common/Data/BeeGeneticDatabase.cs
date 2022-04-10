using System;
using System.Collections.Generic;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public static class Constants
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
            public const string Diurnal = "Diurnal";
            public const string Nocturnal = "Nocturnal";
            public const string Flyer = "Flyer";
            public const string Cave = "Cave";
            public const string Effect = "Effect";
        }

        public static Dictionary<string, Type> StatTypes = new();
        public static Dictionary<Species, int> DefaultSpeciePriorities = new();
        public static Dictionary<Flowers, int> DefaultFlowersPriorities = new();
        public static Dictionary<Effect, int> DefaultEffectPriorities = new();

        static Constants()
        {
            InitStatTypes();
            InitDefaultSpeciePriorities();
            InitDefaultEffectPriorities();
            InitDefaultFlowersPriorities();
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

        private static void InitDefaultEffectPriorities()
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