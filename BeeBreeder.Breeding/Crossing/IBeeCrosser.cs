using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Crossing
{
    public interface IBeeCrosser
    {
        List<Bee> Cross(Bee first, Bee second);
    }
}
