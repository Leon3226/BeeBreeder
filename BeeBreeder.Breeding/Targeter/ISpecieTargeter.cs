using System.Collections.Generic;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Targeter
{
    public interface ISpecieTargeter
    {
        public BeePool Bees { set; }
        public IEnumerable<string> ManualTargets { get; set; }
        public IEnumerable<string> CalculatedTargets { get; }
    }
}
