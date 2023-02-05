using System.Collections.Generic;

namespace BeeBreeder.Common.Data
{
    public interface IGeneDominanceProvider
    {
        Dictionary<string, Dictionary<object, bool>> GenesDominance { get; }
    }
}
