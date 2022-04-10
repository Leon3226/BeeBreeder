using System;

namespace BeeBreeder.Common
{
    public static class RandomGenerator
    {
        public static Random Random { private get; set; } = new(123);

        public static int GenerateInt(int from, int to)
        {
            return Random.Next(from, to);
        }

        public static double Double()
        {
            return Random.NextDouble();
        }
    }
}