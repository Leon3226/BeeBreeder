using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public interface ISpecieClimateRepository
    {
        Dictionary<Species, Climate> SpecieClimates { get; }
    }
}
