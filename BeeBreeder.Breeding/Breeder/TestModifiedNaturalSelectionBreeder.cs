using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto;
using BeeBreeder.Common;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Extensions;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Breeder
{
    public class TestModifiedNaturalSelectionBreeder : NaturalSelectionBreeder
    {
        readonly MutationTree _tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);
        private readonly IStrategyUtils _strategyUtils;
        public int PureMinCount = 5;
        public int ImpureMinCount = 10;
        private List<Species> _selectedTargetSpecies = new();
        private List<Species> _statsTargetSpecies = new();

        public TestModifiedNaturalSelectionBreeder()
        {
            _strategyUtils = new StrategyUtils() {Tree = _tree};
        }

        public override List<Species> TargetSpecies
        {
            get
            {
                var target = _selectedTargetSpecies.Concat(_statsTargetSpecies).Distinct().ToList();
                var intermediateSpecies = _tree.OnlyNecessaryForGettingIfPossibleAndHaveEnough(target,
                    Pool.Bees.ExtractSpecies());
                return intermediateSpecies.Concat(target).Distinct().ToList();
            }
            set => _selectedTargetSpecies = value;
        }

        public override List<(Bee, Bee)> GetBreedingPairs(int count = 0)
        {
            var sw = new Stopwatch();
            
            if (count < 0)
                return new List<(Bee, Bee)>();

            var princesses = Pool.Princesses.ToList();
            if (princesses.Count == 0)
                return new List<(Bee, Bee)>();

            var drones = Pool.Drones.ToList();
            if (drones.Count == 0)
                return new List<(Bee, Bee)>();

            count = count == 0 ? princesses.Sum(x => x.Count) : count;
            
            sw.Start();
            var strategy = _strategyUtils.ImportantTargets(Pool);
            _times.Add(("Strategy", sw.Elapsed));
            sw.Restart();

            List<Species> necessarySpecies = new List<Species>();
            necessarySpecies.AddRange(strategy.Species);
            necessarySpecies = necessarySpecies.Distinct().ToList();
            _statsTargetSpecies = necessarySpecies.ToList();


            var spec = TargetSpecies;
            var toPreserve = princesses.Where(x =>
                spec.Contains(x.Bee.SpecieChromosome.Primary.Value) ||
                spec.Contains(x.Bee.SpecieChromosome.Secondary.Value)).ToList();
            var toPreservePure = toPreserve.Where(x =>
                x.Bee.SpecieChromosome.Clean).ToList();
            var toPreserveImpure = toPreserve.Except(toPreservePure).ToList();

            List<(Bee, Bee)> toReturn = new List<(Bee, Bee)>();

            void InsertPairs(List<BeeStack> pairPrincesses, Func<Bee, List<BeeStack>> partnerFilter)
            {
                var overallCount = pairPrincesses.OverallCount();
                for (int i = 0; i < overallCount; i++)
                {
                    pairPrincesses = pairPrincesses.Where(x => x.Count > 0).ToList();
                    drones = Pool.Drones.ToList();
                    if (drones.Count == 0)
                        return;

                    if (pairPrincesses.Count == 0 || drones.Count == 0)
                        break;

                    var princess = pairPrincesses[RandomGenerator.GenerateInt(0, pairPrincesses.Count)];
                    var partners = partnerFilter(princess.Bee);

                    if (partners.Count == 0)
                        continue;
                    var drone = partners[RandomGenerator.GenerateInt(0, partners.Count)];

                    Pool.RemoveBee(princess.Bee, 1);
                    Pool.RemoveBee(drone.Bee, 1);
                    toReturn.Add((princess.Bee, drone.Bee));
                }
            }
            
            sw.Restart();
            InsertPairs(toPreservePure, GetPreserveImportantPartnersPure);
            _times.Add(("InsPure", sw.Elapsed));
            sw.Restart();
            InsertPairs(toPreserveImpure, GetPreserveImportantPartnersImpure);
            _times.Add(("InsImpure", sw.Elapsed));
            sw.Restart();
            InsertPairs(Pool.Princesses.ToList(), GetPossiblePartners);
            _times.Add(("InsRest", sw.Elapsed));
            sw.Restart();

            _iterations += toReturn.Count;
            return toReturn;
        }

        public override List<(Slot, Bee, Bee)> GetPairsInSlots()
        {
            return base.GetPairsInSlots();
        }

        private List<BeeStack> GetPreserveImportantPartnersPure(Bee bee)
        {
            return GetSpeciePartners(bee, bee.SpecieChromosome.ResultantAttribute);
        }

        public override async Task<List<BeeStack>> NaturalSelectionAsync()
        {
            var breedingTarget = new BreedingTarget();
            foreach (var specie in TargetSpecies)
            {
                breedingTarget.SpeciePriorities[specie] = 100;
            }
            
            var paretoNecessary = ParetoFromNecessaryAsync();
            var optimalDrones = Pool.Drones.ParetoOptimalAsync(breedingTarget);
            
            var survivors = (await optimalDrones).ToList().Concat((await paretoNecessary).ToList()).Distinct().ToList();
            var toRemove = Pool.Drones.Except(survivors).ToList();
            Pool.Drones = survivors;
            return toRemove;
        }

        private List<BeeStack> GetSpeciePartners(Bee bee, Species targetSpecies)
        {
            var partners = bee.Gender == Gender.Princess ? Pool.Drones : Pool.Princesses;

            var purePartners = partners.Where(x =>
            {
                var sc = x.Bee.SpecieChromosome;
                return sc.Clean && sc.Primary.Value == targetSpecies;
            }).ToList();
            
            var pureCount = purePartners.OverallCount();
            if (pureCount == 0)
            {
                var impurePartners = partners.Where(x =>
                {
                    var sc = x.Bee.SpecieChromosome;
                    return sc.Primary.Value == targetSpecies ||
                           sc.Secondary.Value == targetSpecies;
                }).ToList();
                var impureCount = impurePartners.OverallCount();

                if (impureCount == 0 || impureCount >= ImpureMinCount)
                {
                    return partners;
                }
                else
                {
                    partners = impurePartners;
                    return partners;
                }
            }

            if (pureCount < PureMinCount)
            {
                partners = purePartners;
                return partners;
            }

            return partners;
        }
        private List<BeeStack> GetPreserveImportantPartnersImpure(Bee bee)
        {
            var spec = TargetSpecies;
            return GetSpeciePartners(bee,
                spec.Contains(bee.SpecieChromosome.Primary.Value)
                    ? bee.SpecieChromosome.Primary.Value
                    : bee.SpecieChromosome.Secondary.Value);
        }

        private List<BeeStack> GetPossiblePartners(Bee bee)
        {
            var partners = bee.Gender == Gender.Princess ? Pool.Drones : Pool.Princesses;

            void ExcludeParetoEqualWithDifferentSpecies()
            {
                var specie1 = (SpecieChromosome) bee[BeeGeneticDatabase.StatNames.Specie];
                for (int i = 0; i < partners.Count; i++)
                {
                    var partner = partners[i];

                    var specie2 = (SpecieChromosome) partner.Bee[BeeGeneticDatabase.StatNames.Specie];
                    if (((specie1.Clean &&
                          specie2.Clean) ||
                         !(specie1.Primary.Value == specie2.Primary.Value &&
                           specie1.Secondary.Value == specie2.Secondary.Value &&
                           specie1.Primary.Value == specie2.Primary.Value)) &&
                        !_tree.PossibleResults(new List<Species>()
                            {specie1.Primary.Value, specie2.Primary.Value}).Any() &&
                        !_tree.PossibleResults(new List<Species>()
                            {specie1.Primary.Value, specie2.Secondary.Value}).Any() &&
                        !_tree.PossibleResults(new List<Species>()
                            {specie1.Secondary.Value, specie2.Secondary.Value}).Any()
                    )
                    {
                        var isEqual = true;
                        foreach (var gene in bee.Genotype.Genes)
                        {
                            if (gene.Key == BeeGeneticDatabase.StatNames.Specie)
                                continue;

                            var secondGene = partner.Bee[gene.Key];
                            isEqual =
                                (gene.Value.Primary.Equals(secondGene.Primary) &&
                                 gene.Value.Secondary.Equals(secondGene.Secondary)) ||
                                (gene.Value.Primary.Equals(secondGene.Secondary) &&
                                 gene.Value.Secondary.Equals(secondGene.Primary));
                            if (!isEqual)
                                break;
                        }

                        if (isEqual)
                            partners.Remove(partner);
                    }
                }
            }

            ExcludeParetoEqualWithDifferentSpecies();

            return partners;
        }
    }
}