using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Positioning;

namespace BeeBreeder.Breeding.Positioning
{
    public interface IPositionsController
    {
        List<(ApiaryPosition position, (Bee Princess, Bee Drone))> Assign(IEnumerable<(Bee Princess, Bee Drone)> pairs, IEnumerable<TransposerData> transposerData, IEnumerable<ApiaryPosition> avaliablePositions);
    }
}
