using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Flusher
{
    public class ExtendedNaturalSelectionFlusher : NaturalSelectionFlusher
    {
        public override async Task<List<BeeStack>> NaturalSelectionAsync(BeePool bees)
        {
            var breedingTarget = new BreedingTarget();
            foreach (var specie in TargetSpecies)
            {
                breedingTarget.SpeciePriorities[specie] = 100;
            }

            var paretoNecessary = ParetoFromNecessaryAsync(bees);
            var optimalDrones = bees.Drones.ParetoOptimalAsync(breedingTarget);

            var survivors = (await optimalDrones).ToList().Concat((await paretoNecessary).ToList()).Distinct().ToList();
            var toRemove = bees.Drones.Except(survivors).ToList();
            bees.Drones = survivors;
            return toRemove;
        }
    }
}
