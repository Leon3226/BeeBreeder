using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Targeter
{
    public interface ISpecieTargeter
    {
        public BeePool Bees { set; }
        public IEnumerable<Species> ManualTargets { get; set; }
        public IEnumerable<Species> CalculatedTargets { get; }
    }
}
