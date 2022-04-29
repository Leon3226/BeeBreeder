using System.Collections.Generic;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public interface ISpecieClimateRepository
    {
        Dictionary<string, Climate> SpecieClimates { get; }
    }
}
