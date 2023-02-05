using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Data.Extensions
{
    internal static class StringExtension
    {
        public static int CalculateVolume(this string input)
        {
            return input.Split('x').Select(x => int.Parse(x)).Aggregate(1, (x, y) => x * y);
        }
    }
}
