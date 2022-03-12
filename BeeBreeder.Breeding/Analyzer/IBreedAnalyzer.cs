using BeeBreeder.Common.Model.Bees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Breeding.Analyzer
{
    public interface IBreedAnalyzer
    {
        List<(Bee Princess, Bee Drone)> GetBreedingPairs(BeePool bees, int count = 0);
    }
}
