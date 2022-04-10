using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeeBreeder.Common.Model.Genetics
{
    [JsonArray]
    public class ChromosomeDictionary : Dictionary<string, IChromosome>
    {

    }
}
