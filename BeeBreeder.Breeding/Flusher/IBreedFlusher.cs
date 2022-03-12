using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Flusher
{
    public interface IBreedFlusher
    {
        IEnumerable<BeeStack> ToFlush(BeePool bees);
    }
}
