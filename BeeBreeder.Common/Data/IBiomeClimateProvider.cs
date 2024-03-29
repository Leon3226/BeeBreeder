﻿using System.Collections.Generic;
using BeeBreeder.Common.Model.Environment;

namespace BeeBreeder.Common.Data
{
    public interface IBiomeClimateProvider
    {
        Dictionary<Biome, Climate> BiomeClimates { get; }
    }
}
