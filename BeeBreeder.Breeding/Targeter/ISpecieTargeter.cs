using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Targeter
{
    public interface ISpecieTargeter
    {
        public BeePool Bees { set; }
        public IEnumerable<Species> ManualTargets { get; set; }
        public IEnumerable<Species> CalculatedTargets { get; }
    }
}
