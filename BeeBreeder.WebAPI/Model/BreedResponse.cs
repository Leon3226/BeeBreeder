using System.Collections.Generic;
using BeeBreeder.Common.Model.Positioning;

namespace BeeBreeder.WebAPI.Model
{
    public class BreedResponse
    {
        public List<InventoryPosition> Flush;
        public List<(InventoryPosition, InventoryPosition)> Breed;
    }
}