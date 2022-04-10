using BeeBreeder.Common.Model.Bees;
using System.Collections.Generic;

namespace BeeBreeder.Breeding.Analyzer
{
    public interface IBreedAnalyzer
    {
        List<(Bee Princess, Bee Drone)> GetBreedingPairs(BeePool bees, int count = 0);
    }
}
