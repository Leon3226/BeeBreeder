using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Breeding.Breeder
{
    public class NaturalSelectionBreeder : BeeBreederBase
    {
        readonly Random _rand = new();
        private MutationTree _tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);

        public int IterationsBetweenNaturalSelectionClears = 3;
        public bool ClearDuplicates = true;
        public int IterationsBetweenDuplicatesClears = 50;
        public int AllowedDuplicatesCount = 2;
        public virtual List<Species> TargetSpecies { get; set; } = new() {Species.Imperial};

        private int _iterationsFromPrevNatSelection = 0;
        private int _iterationsFromPrevDuplClear = 0;

        private bool NeedToDuplClear => ClearDuplicates && _iterations - _iterationsFromPrevDuplClear > IterationsBetweenDuplicatesClears;
        private bool NeedToNatSelection => _iterations - _iterationsFromPrevNatSelection > IterationsBetweenNaturalSelectionClears;
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
                var princess = princesses[_rand.Next(0, princesses.Count)];
                var drone = drones[_rand.Next(0, drones.Count)];
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

        protected IEnumerable<BeeStack> ParetoFromNecessary()
        {
            var necessarySpecies = TargetSpecies;

            var paretoBees = new List<BeeStack>();
            foreach (var specie in necessarySpecies)
            {
                
                var target = new BreedingTarget();
                target.SpeciePriorities[specie] = 100;
                var bees = Pool.Drones.Except(paretoBees).Where(x =>
                        x.Bee[StatNames.Specie].Primary.Value.Equals(specie) ||
                        x.Bee[StatNames.Specie].Secondary.Value.Equals(specie)).ToList();
                var pareto = bees.Select(x => x.Bee).ParetoOptimal(target);
                paretoBees.AddRange(bees.Where(x => pareto.Contains(x.Bee)));
            }

            return paretoBees.Distinct();
        }
        public override IEnumerable<BeeStack> ToFlush()
        {
            if (NeedToNatSelection)
            {
                _iterationsFromPrevNatSelection = _iterations;
                return NaturalSelection();
            }

            return new List<BeeStack>();
        }
        public virtual List<BeeStack> NaturalSelection()
        {
            var paretoNecessary = ParetoFromNecessary().ToList();
            var optimalDrones = Pool.Drones.Except(paretoNecessary).ToList().ParetoOptimal().Distinct().ToList();
            var count = Pool.Drones.Count - optimalDrones.Count;
            var survivors = optimalDrones.Concat(paretoNecessary).ToList();
            var toRemove = Pool.Drones.Except(survivors).ToList();
            Pool.Drones = survivors;
            return toRemove;
        }
    }
}