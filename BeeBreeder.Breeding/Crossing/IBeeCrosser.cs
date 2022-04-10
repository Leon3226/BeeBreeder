using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Crossing
{
    public interface IBeeCrosser
    {
        List<Bee> Cross(Bee first, Bee second);
    }
}
