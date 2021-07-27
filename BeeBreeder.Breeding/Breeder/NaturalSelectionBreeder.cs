using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Breeder
{
    public class NaturalSelectionBreeder : IBeeBreeder
    {
        readonly Random _rand = new();
        private MutationTree _tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);

        public int IterationsBetweenNaturalSelectionClears = 10;
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
            }
        }

        public List<(Bee, Bee)> GetBreedingPairs(int count = 0)
        {
            if (count < 0)
                return new List<(Bee, Bee)>();

            var princesses = Pool.Princesses.ToList();
            if (princesses.Count == 0)
                return new List<(Bee, Bee)>();
            ;
            var drones = Pool.Drones.ToList();
            if (drones.Count == 0)
                return new List<(Bee, Bee)>();

            count = count == 0 ? princesses.Count : count;

            List<(Bee, Bee)> toReturn = new List<(Bee, Bee)>();

            for (int i = 0; i < count; i++)
            {
                if (princesses.Count == 0 || drones.Count == 0)
                    break;
                var princess = princesses[_rand.Next(0, princesses.Count)];
                var drone = drones[_rand.Next(0, drones.Count)];
                princesses.Remove(princess);
                drones.Remove(drone);
                toReturn.Add((princess, drone));
            }

            _iterations += toReturn.Count;
            return toReturn;
        }

        private IEnumerable<Bee> ParetoFromNecessary()
        {
            var necessarySpecies = _tree.OnlyNecessaryForGetting(
                TargetSpecies, Pool.Drones.Select(x =>
                        ((Chromosome<Species>) x.Genotype[BeeGeneticDatabase.StatNames.Specie]).ResultantAttribute)
                    .Distinct()
                    .ToList());

            var paretoBees = new List<Bee>();
            foreach (var specie in necessarySpecies)
            {
                var bees = Pool.Drones.Except(paretoBees).Where(x =>
                        x.Genotype[BeeGeneticDatabase.StatNames.Specie].Primary.Value.Equals(specie) ||
                        x.Genotype[BeeGeneticDatabase.StatNames.Specie].Secondary.Value.Equals(specie)).ToList()
                    .ParetoOptimal();
                paretoBees.AddRange(bees);
            }

            return paretoBees.Distinct();
        }

        public IEnumerable<Bee> ToFlush()
        {
            var res = new List<Bee>();
            if (NeedToDuplClear)
            {
                _iterationsFromPrevDuplClear = _iterations;
                res.AddRange(Pool.RemoveDroneDuplicates(AllowedDuplicatesCount));
            }
            
            if (NeedToNatSelection)
            {
                _iterationsFromPrevNatSelection = _iterations;
                res.AddRange(NaturalSelection());
            }

            return res;
        }
        public List<Bee> NaturalSelection()
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