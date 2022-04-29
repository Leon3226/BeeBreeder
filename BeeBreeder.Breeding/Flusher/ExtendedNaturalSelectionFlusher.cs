using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Comparison.Pareto;
using BeeBreeder.Breeding.Targeter;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Flusher
{
    public class ExtendedNaturalSelectionFlusher : NaturalSelectionFlusher
    {
        public ExtendedNaturalSelectionFlusher(ISpecieTargeter specieTargeter, IParetoComparer paretoComparer)
            : base(specieTargeter, paretoComparer)
        {

        }
        public override async Task<List<BeeStack>> NaturalSelectionAsync(BeePool bees)
        {
            SpecieTargeter.Bees = bees;
            var necessarySpecies = SpecieTargeter.CalculatedTargets.ToList();

            var paretoNecessary = ParetoFromNecessaryAsync(bees);
            var optimalDrones = ParetoComparer.ParetoOptimalAsync(bees.Drones);

            var survivors = (await optimalDrones).ToList().Concat((await paretoNecessary).ToList()).Distinct().ToList();
            var toRemove = bees.Drones.Except(survivors).ToList();
            bees.Drones = survivors;
            return toRemove;
        }

        
    }
}
