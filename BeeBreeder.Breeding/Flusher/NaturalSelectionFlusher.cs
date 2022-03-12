using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Extensions;

namespace BeeBreeder.Breeding.Flusher
{
    public class NaturalSelectionFlusher : IBreedFlusher
    {
        public int ClearDirtySpeciesAt = 5;
        public virtual List<Species> TargetSpecies { get; set; } = new() { Species.Imperial };

        public IEnumerable<BeeStack> ToFlush(BeePool bees)
        {
            var toRemove = new List<BeeStack>();
            toRemove.AddRange(DirtySpecies(bees));
            toRemove.AddRange(NaturalSelectionAsync(bees).Result.ToList());

            return toRemove;
        }

        public virtual async Task<List<BeeStack>> NaturalSelectionAsync(BeePool bees)
        {
            var paretoNecessary = (await ParetoFromNecessaryAsync(bees)).ToList();
            var optimalDrones = (await bees.Drones.Except(paretoNecessary).ToList().ParetoOptimalAsync()).Distinct()
                .ToList();
            var count = bees.Drones.Count - optimalDrones.Count;
            var survivors = optimalDrones.Concat(paretoNecessary).ToList();
            var toRemove = bees.Drones.Except(survivors).ToList();
            bees.Drones = survivors;
            return toRemove;
        }

        protected virtual async Task<IEnumerable<BeeStack>> ParetoFromNecessaryAsync(BeePool bees)
        {
            var necessarySpecies = TargetSpecies;

            var paretoBees = new List<BeeStack>();
            foreach (var specie in necessarySpecies)
            {
                var target = new BreedingTarget { SpeciePriorities = { [specie] = 100 } };
                await Task.Run(async () =>
                {
                    var result = bees.Drones.Except(paretoBees).Where(x =>
                        x.Bee[BeeGeneticDatabase.StatNames.Specie].Primary.Value.Equals(specie) ||
                        x.Bee[BeeGeneticDatabase.StatNames.Specie].Secondary.Value.Equals(specie)).ToList();

                    var pareto = await result.Select(x => x.Bee).ParetoOptimalAsync(target);

                    paretoBees.AddRange(result.Where(x => pareto.Contains(x.Bee)));
                });
            }

            return paretoBees.Distinct();
        }

        public virtual List<BeeStack> DirtySpecies(BeePool bees, int clearAt = -1)
        {
            if (clearAt < 0)
                clearAt = ClearDirtySpeciesAt;

            var dirtyToCheck = new List<BeeStack>();
            var toRemove = new List<BeeStack>();
            var cleanSpecies = new Dictionary<Species, int>();

            bees.Drones.ExtractSpecies().Select(x => x.Key).ToList().ForEach(x => cleanSpecies.Add(x, 0));

            foreach (var droneStack in bees.Drones)
            {
                var specieChromosome = droneStack.Bee.SpecieChromosome;
                if (specieChromosome.Clean)
                {
                    cleanSpecies[specieChromosome.Primary.Value] += droneStack.Count;
                }
                else
                {
                    dirtyToCheck.Add(droneStack);
                }
            }

            foreach (var droneStack in dirtyToCheck)
            {
                var sc = droneStack.Bee.SpecieChromosome;
                if (cleanSpecies[sc.Primary.Value] > clearAt && cleanSpecies[sc.Secondary.Value] > clearAt)
                {
                    toRemove.Add(droneStack);
                    bees.Bees.Remove(droneStack);
                }
            }

            return toRemove;
        }
    }
}
