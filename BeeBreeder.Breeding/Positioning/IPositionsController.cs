using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Positioning;

namespace BeeBreeder.Breeding.Positioning
{
    public interface IPositionsController
    {
        BeePool Filter(BeePool bees, IEnumerable<TransposerBiome> transposerBiomes, IEnumerable<ApiaryPosition> avaliablePositions);
    }
}
