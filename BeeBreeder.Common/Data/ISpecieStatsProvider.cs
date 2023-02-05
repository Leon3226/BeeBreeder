using System.Collections.Generic;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public interface ISpecieStatsProvider
    {
        Dictionary<string, BeeInitialStats> SpecieStats { get; }
    }
}
