using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Breeding.Targeter;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.Flusher
{
    public class ExtendedNaturalSelectionFlusher : NaturalSelectionFlusher
    {
        public BreedingTarget BreedingTarget { get; set; }

        public ExtendedNaturalSelectionFlusher(ISpecieTargeter specieTargeter)
            : base(specieTargeter)
        {
            //TODO: move
            BreedingTarget = new BreedingTarget() {SpeciePriorities = Constants.DefaultSpeciePriorities};
        }
        public override async Task<List<BeeStack>> NaturalSelectionAsync(BeePool bees)
        {
            SpecieTargeter.Bees = bees;
            var necessarySpecies = SpecieTargeter.CalculatedTargets.ToList();
            var breedingTarget = BreedingTarget.Copy();
            foreach (var specie in necessarySpecies)
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
