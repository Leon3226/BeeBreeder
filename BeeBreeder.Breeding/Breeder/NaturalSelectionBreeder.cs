using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Extensions;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Breeding.Breeder
{
    public class NaturalSelectionBreeder : BeeBreederBase
    {
        private MutationTree _tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);

        public int IterationsBetweenNaturalSelectionClears = 3;
        public int ClearDirtySpeciesAt = 5;
        public bool ClearDuplicates = true;
        public int IterationsBetweenDuplicatesClears = 50;
        public int AllowedDuplicatesCount = 2;
        public virtual List<Species> TargetSpecies { get; set; } = new() {Species.Imperial};

        private int _iterationsFromPrevNatSelection = 0;
        private int _iterationsFromPrevDuplClear = 0;

        private bool NeedToDuplClear => ClearDuplicates &&
                                        _iterations - _iterationsFromPrevDuplClear > IterationsBetweenDuplicatesClears;

        private bool NeedToNatSelection =>
            _iterations - _iterationsFromPrevNatSelection > IterationsBetweenNaturalSelectionClears;

        protected int _iterations = 0;

        public override List<(Bee, Bee)> GetBreedingPairs(int count = 0)
        {
            if (count < 0)
                return new List<(Bee, Bee)>();

            var princesses = Pool.Princesses.ToList();
            if (princesses.Count == 0)
                return new List<(Bee, Bee)>();

            var drones = Pool.Drones.ToList();
            if (drones.Count == 0)
                return new List<(Bee, Bee)>();

            count = count == 0 ? princesses.Sum(x => x.Count) : count;

            List<(Bee, Bee)> toReturn = new List<(Bee, Bee)>();

            for (int i = 0; i < count; i++)
            {
                princesses = Pool.Princesses.ToList();
                if (princesses.Count == 0)
                    return new List<(Bee, Bee)>();

                drones = Pool.Drones.ToList();
                if (drones.Count == 0)
                    return new List<(Bee, Bee)>();

                if (princesses.Count == 0 || drones.Count == 0)
                    break;
                var princess = princesses[RandomGenerator.GenerateInt(0, princesses.Count)];
                var drone = drones[RandomGenerator.GenerateInt(0, drones.Count)];
                Pool.RemoveBee(princess.Bee, 1);
                Pool.RemoveBee(drone.Bee, 1);
                toReturn.Add((princess.Bee, drone.Bee));
            }

            _iterations += toReturn.Count;
            return toReturn;
        }

        public override List<(Slot, Bee, Bee)> GetPairsInSlots()
        {
            throw new NotImplementedException();
        }

        protected async Task<IEnumerable<BeeStack>> ParetoFromNecessaryAsync()
        {
            var necessarySpecies = TargetSpecies;

            var paretoBees = new List<BeeStack>();
            foreach (var specie in necessarySpecies)
            {
                var target = new BreedingTarget {SpeciePriorities = {[specie] = 100}};
                await Task.Run(async () =>
                {
                    var bees = Pool.Drones.Except(paretoBees).Where(x =>
                        x.Bee[StatNames.Specie].Primary.Value.Equals(specie) ||
                        x.Bee[StatNames.Specie].Secondary.Value.Equals(specie)).ToList();

                    var pareto = bees.Select(x => x.Bee).ParetoOptimal(target);

                    paretoBees.AddRange(bees.Where(x => pareto.Contains(x.Bee)));
                });
            }

            return paretoBees.Distinct();
        }

        public override IEnumerable<BeeStack> ToFlush()
        {
            var toRemove = new List<BeeStack>();
            toRemove.AddRange(DirtySpecies());
            if (NeedToNatSelection)
            {
                _iterationsFromPrevNatSelection = _iterations;
                toRemove.AddRange(NaturalSelectionAsync().Result.ToList());
            }

            return toRemove;
        }

        public virtual List<BeeStack> DirtySpecies(int cleatAt = -1)
        {
            if (cleatAt < 0)
                cleatAt = ClearDirtySpeciesAt;

            var dirtyToCheck = new List<BeeStack>();
            var toRemove = new List<BeeStack>();
            var cleanSpecies = new Dictionary<Species, int>();

            Pool.Drones.ExtractSpecies().Select(x => x.Key).ToList().ForEach(x => cleanSpecies.Add(x, 0));

            foreach (var droneStack in Pool.Drones)
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
                if (cleanSpecies[sc.Primary.Value] > cleatAt && cleanSpecies[sc.Secondary.Value] > cleatAt)
                {
                    toRemove.Add(droneStack);
                    Pool.Bees.Remove(droneStack);
                }
            }

            return toRemove;
        }

        public virtual async Task<List<BeeStack>> NaturalSelectionAsync()
        {
            var paretoNecessary = (await ParetoFromNecessaryAsync()).ToList();
            var optimalDrones = (await Pool.Drones.Except(paretoNecessary).ToList().ParetoOptimalAsync()).Distinct()
                .ToList();
            var count = Pool.Drones.Count - optimalDrones.Count;
            var survivors = optimalDrones.Concat(paretoNecessary).ToList();
            var toRemove = Pool.Drones.Except(survivors).ToList();
            Pool.Drones = survivors;
            return toRemove;
        }
    }
}