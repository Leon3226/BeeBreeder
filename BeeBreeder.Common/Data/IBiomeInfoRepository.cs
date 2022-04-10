using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Common.Model.Environment;

namespace BeeBreeder.Common.Data
{
    public interface IBiomeInfoRepository
    {
        Dictionary<Biome, Climate> BiomeClimates { get; }
    }
}
