using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Genetics;
using StatNames = BeeBreeder.Common.AlleleDatabase.Bee.BeeGeneticDatabase.StatNames;

namespace BeeBreeder.Breeding.Breeder
{
    public class NaturalSelectionBreeder : IBeeBreeder
    {
        readonly Random _rand = new();
        private MutationTree _tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);

        public int IterationsBetweenNaturalSelectionClears = 3;
        public bool ClearDuplicates = true;
        public int IterationsBetweenDuplicatesClears = 50;
        public int AllowedDuplicatesCount = 2;
        public List<Species> TargetSpecies = new() {Species.Imperial};

        private int _iterationsFromPrevNatSelection = 0;
        private int _iterationsFromPrevDuplClear = 0;

        private bool NeedToDuplClear => ClearDuplicates && _iterations - _iterationsFromPrevDuplClear > IterationsBetweenDuplicatesClears;
        private bool NeedToNatSelection => _iterations - _iterationsFromPrevNatSelection > IterationsBetweenNaturalSelectionClears;
        private int _iterations = 0;
        public BeePool Pool { get; set; }

        public void Breed(int iterations)
        {
            if (iterations < 0) return;
            for (int i = 0; i < iterations;)
            {
                var pairs = GetBreedingPairs();
                if (pairs.Count == 0)
                    break;
                
                i += pairs.Count;

                ToFlush();
                pairs.ForEach(x => Pool.Cross(x.Item1, x.Item2));
                Pool.CompactDuplicates();
            }
        }

        public List<(Bee, Bee)> GetBreedingPairs(int count = 0)
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

        private IEnumerable<BeeStack> ParetoFromNecessary()
        {
            var necessarySpecies = _tree.OnlyNecessaryForGetting(
                TargetSpecies, Pool.Drones.Select(x =>
                        ((Chromosome<Species>) x.Bee.Genotype[StatNames.Specie]).ResultantAttribute)
                    .Distinct()
                    .ToList());

            var paretoBees = new List<BeeStack>();
            foreach (var specie in necessarySpecies)
            {
                var bees = Pool.Drones.Except(paretoBees).Where(x =>
                        x.Bee.Genotype[StatNames.Specie].Primary.Value.Equals(specie) ||
                        x.Bee.Genotype[StatNames.Specie].Secondary.Value.Equals(specie)).ToList();
                var pareto = bees.Select(x => x.Bee).ParetoOptimal();
                paretoBees.AddRange(bees.Where(x => pareto.Contains(x.Bee)));
            }

            return paretoBees.Distinct();
        }

        public IEnumerable<BeeStack> ToFlush()
        {
            if (NeedToNatSelection)
            {
                _iterationsFromPrevNatSelection = _iterations;
                return NaturalSelection();
            }

            return new List<BeeStack>();
        }
        public List<BeeStack> NaturalSelection()
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