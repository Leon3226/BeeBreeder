using System.Collections.Generic;
using BeeBreeder.Common.Model.Environment;

namespace BeeBreeder.Common.Data
{
    public interface IBiomeInfoProvider
    {
        Dictionary<Biome, Climate> BiomeClimates { get; }
    }
}
