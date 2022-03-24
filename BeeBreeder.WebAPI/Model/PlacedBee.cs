using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Positioning;

namespace BeeBreeder.WebAPI.Model
{
    public class PlacedBee
    {
        public GameBeeModel Bee;
        public ApiaryPosition BeePosition;
    }
}