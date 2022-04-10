using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Environment;

namespace BeeBreeder.Breeding.EnvironmentMatching
{
    public interface IEnvironmentMatcher
    {
        (List<Temperature> temperatures, List<Humidity> humidities) AcceptableConditions(Bee bee);
        List<Biome> AcceptableBiomes(Bee bee);

        bool CanLiveIn(Bee bee, Biome biome);
        bool CanLiveIn(Bee bee, Climate climate);

    }
}
