using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Positioning;

namespace BeeBreeder.Breeding.Positioning
{
    public class PositionsController : IPositionsController
    {
        public BeePool Filter(BeePool bees, IEnumerable<TransposerBiome> transposerBiomes, IEnumerable<ApiaryPosition> avaliablePositions)
        {
            var biomesAvaliable = transposerBiomes.Where(x => avaliablePositions.Any(pos => pos.Trans == x.Transposer)).Select(x => x.Biome).ToList();
            return new BeePool()
            {
                //Bees = bees.Bees.Where(x => biomesAvaliable.All(biome => x.Bee.CanLiveIn(biome))).Select(x => new BeeStack(x.Bee, x.Count)).ToList()
            };
        }
    }
}
