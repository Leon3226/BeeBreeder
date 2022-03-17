using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeeBreeder.Common.Model.Genetics
{
    [JsonArray]
    public class ChromosomeDictionary : Dictionary<string, IChromosome>
    {

    }
}
