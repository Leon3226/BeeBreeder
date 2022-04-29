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
        public static Dictionary<string, int> DefaultSpeciePriorities = new();
        public static Dictionary<string, int> DefaultFlowersPriorities = new();
        public static Dictionary<string, int> DefaultEffectPriorities = new();
        public static List<(string Name, int Index, double ChanceModifier)> SpeedStatNames { get; } = new();
        public static List<(string Name, int Index, int Cycles)> LifespanStatNames { get; } = new();
        public static List<(string Name, int Index, double ChanceModifier)> PollinationStatNames { get; } = new();

        static Constants()
        {
            InitStatTypes();
            InitDefaultSpeciePriorities();
            InitDefaultEffectPriorities();
            InitDefaultFlowersPriorities();
            InitSpeedStatNames();
            InitLifespanStatNames();
            InitPollinationStatNames();
        }


        private static void InitSpeedStatNames()
        {
            SpeedStatNames.AddRange(new[]
            {
                ("slowest", 1, 3.0),
                ("slower", 2, 6.0),
                ("slow", 3, 8.0),
                ("normal", 4, 1.00),
                ("fast", 5, 1.20),
                ("faster", 6, 1.40),
                ("fastest", 7, 1.70),
                ("blinding", 8, 2.00),
                ("robotic", 9, 2.50),
                ("accelerated", 10, 4.00)

            });
        }

        private static void InitLifespanStatNames()
        {
            LifespanStatNames.AddRange(new[]
            {
                ("blink", 0, 2),
                ("shortest", 1, 10),
                ("shorter", 2, 20),
                ("short", 3, 30),
                ("shortened", 4, 35),
                ("normal", 5, 40),
                ("elongated", 6, 45),
                ("long", 7, 50),
                ("longer", 8, 60),
                ("longest", 9, 70),
                ("eon", 10, 600),
                ("immortal", 11, 100000)
            });
        }

        private static void InitPollinationStatNames()
        {
            PollinationStatNames.AddRange(new[]
            {
                ("slowest", 1, 0.5),
                ("slower", 2, 1.0),
                ("slow", 3, 1.5),
                ("normal", 4, 2.0),
                ("average", 4, 2.0),
                ("fast", 5, 2.5),
                ("faster", 6, 3.0),
                ("fastest", 7, 3.5),
                ("maximum", 8, 9.9),
                ("naturalistic", 9, 2.40)
            });
        }

        private static void InitDefaultSpeciePriorities()
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

        private static void InitDefaultEffectPriorities()
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

        private static void InitDefaultFlowersPriorities()
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

        private static void InitStatTypes()
        {
            StatTypes = new Dictionary<string, Type>()
            {
                {StatNames.Specie, typeof(string)},
                {StatNames.Lifespan, typeof(int)},
                {StatNames.Speed, typeof(int)},
                {StatNames.Pollination, typeof(int)},
                {StatNames.Flowers, typeof(string)},
                {StatNames.Fertility, typeof(int)},
                {StatNames.Area, typeof(int)},
                {StatNames.TempTolerance, typeof(Adaptation)},
                {StatNames.HumidTolerance, typeof(Adaptation)},
                {StatNames.Diurnal, typeof(int)},
                {StatNames.Nocturnal, typeof(int)},
                {StatNames.Flyer, typeof(int)},
                {StatNames.Cave, typeof(int)},
                {StatNames.Effect, typeof(string)}
            };
        }
    }
}