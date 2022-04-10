using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Common.Data.Model;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Common.Data
{
    public interface ISpecieStatsRepository
    {
        Dictionary<Species, BeeInitialStats> SpecieStats { get; }
    }
}
