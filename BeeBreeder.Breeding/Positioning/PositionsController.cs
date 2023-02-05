using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.EnvironmentMatching;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Positioning;

namespace BeeBreeder.Breeding.Positioning
{
    public class PositionsController : IPositionsController
    {
        private readonly IEnvironmentMatcher _environmentMatcher;

        public PositionsController(IEnvironmentMatcher environmentMatcher)
        {
            _environmentMatcher = environmentMatcher;
        }

        public List<(InventoryPosition position, (Bee Princess, Bee Drone))> Assign(IEnumerable<(Bee Princess, Bee Drone)> pairs, IEnumerable<TransposerData> transposersData, IEnumerable<InventoryPosition> avaliablePositions)
        {
            var positions = new List<(InventoryPosition position, (Bee Princess, Bee Drone))>();
            var avaliablePairs = new List<(Bee Princess, Bee Drone)>(pairs);
            foreach (var avaliablePosition in avaliablePositions)
            {
                var tranposerData = transposersData.Single(x => x.Transposer == avaliablePosition.Trans);

                //TODO: It is possible to opmimize algorythm to maximize output. 
                var pair = avaliablePairs.FirstOrDefault(x => 
                _environmentMatcher.CanLiveIn(x.Princess, tranposerData.Biome) &&
                (tranposerData.IsRoofed == false || x.Princess.ChromosomeOf<int>(Constants.StatNames.Cave).ResultantAttribute == 1) &&
                tranposerData.Flowers.Contains(x.Princess.ChromosomeOf<string>(Constants.StatNames.Flowers).ResultantAttribute));
                if (pair != default)
                {
                    avaliablePairs.Remove(pair);
                    positions.Add((avaliablePosition, pair));
                }
            }

            return positions;
        }
    }
}
