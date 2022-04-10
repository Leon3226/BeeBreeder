using System.Collections.Generic;

namespace BeeBreeder.Common.Data
{
    public interface IGeneDominanceRepository
    {
        Dictionary<string, Dictionary<object, bool>> GenesDominance { get; }
    }
}
