using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy;
using BeeBreeder.Common;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Extensions;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Analyzer
{
    public class ExtendedNaturalSelectionAnalyzer : IBreedAnalyzer
    {
        readonly MutationTree _tree = MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations);
        private readonly IStrategyUtils _strategyUtils;

        private List<Species> _selectedTargetSpecies = new();
        private List<Species> _statsTargetSpecies = new();

        public int PureMinCount = 5;
        public int ImpureMinCount = 10;
        public List<Species> TargetSpecies
        {
            set => _selectedTargetSpecies = value;
        }

        public ExtendedNaturalSelectionAnalyzer(IStrategyUtils strategyUtils)
        {
            _strategyUtils = strategyUtils;
        }

        List<Species> GetTargetSpecies(BeePool bees)
        {
            var target = _selectedTargetSpecies.Concat(_statsTargetSpecies).Distinct().ToList();
            var intermediateSpecies = _tree.OnlyNecessaryForGettingIfPossibleAndHaveEnough(target,
                bees.Bees.ExtractSpecies());
            return intermediateSpecies.Concat(target).Distinct().ToList();
        }

        public List<(Bee Princess, Bee Drone)> GetBreedingPairs(BeePool bees, int count = 0)
        {
            if (count < 0)
                return new List<(Bee, Bee)>();

            var princesses = bees.Princesses.ToList();
            if (princesses.Count == 0)
                return new List<(Bee, Bee)>();

            var drones = bees.Drones.ToList();
            if (drones.Count == 0)
                return new List<(Bee, Bee)>();

            count = count == 0 ? princesses.Sum(x => x.Count) : count;
            
            var strategy = _strategyUtils.ImportantTargets(bees);

            List<Species> necessarySpecies = new List<Species>();
            necessarySpecies.AddRange(strategy.Species);
            necessarySpecies = necessarySpecies.Distinct().ToList();
            _statsTargetSpecies = necessarySpecies.ToList();


            var spec = GetTargetSpecies(bees);
            var toPreserve = princesses.Where(x =>
                spec.Contains(x.Bee.SpecieChromosome.Primary.Value) ||
                spec.Contains(x.Bee.SpecieChromosome.Secondary.Value)).ToList();
            var toPreservePure = toPreserve.Where(x =>
                x.Bee.SpecieChromosome.Clean).ToList();
            var toPreserveImpure = toPreserve.Except(toPreservePure).ToList();

            List<(Bee, Bee)> toReturn = new List<(Bee, Bee)>();

            void InsertPairs(BeePool bees, List< BeeStack> pairPrincesses, Func<BeePool, Bee, List<BeeStack>> partnerFilter)
            {
                var overallCount = pairPrincesses.OverallCount();
                for (int i = 0; i < overallCount; i++)
                {
                    pairPrincesses = pairPrincesses.Where(x => x.Count > 0).ToList();
                    drones = bees.Drones.ToList();
                    if (drones.Count == 0)
                        return;

                    if (pairPrincesses.Count == 0 || drones.Count == 0)
                        break;

                    var princess = pairPrincesses[RandomGenerator.GenerateInt(0, pairPrincesses.Count)];
                    var partners = partnerFilter(bees, princess.Bee);

                    if (partners.Count == 0)
                        continue;
                    var drone = partners[RandomGenerator.GenerateInt(0, partners.Count)];

                    bees.RemoveBee(princess.Bee, 1);
                    bees.RemoveBee(drone.Bee, 1);
                    toReturn.Add((princess.Bee, drone.Bee));
                }
            }

            InsertPairs(bees, toPreservePure, GetPreserveImportantPartnersPure);
            InsertPairs(bees, toPreserveImpure, GetPreserveImportantPartnersImpure);
            InsertPairs(bees, bees.Princesses.ToList(), GetPossiblePartners);
            
            return toReturn;
        }

        private List<BeeStack> GetPreserveImportantPartnersPure(BeePool bees, Bee bee)
        {
            return GetSpeciePartners(bees, bee, bee.SpecieChromosome.ResultantAttribute);
        }

        private List<BeeStack> GetSpeciePartners(BeePool bees, Bee bee, Species targetSpecies)
        {
            var partners = bee.Gender == Gender.Princess ? bees.Drones : bees.Princesses;

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
        private List<BeeStack> GetPreserveImportantPartnersImpure(BeePool bees, Bee bee)
        {
            var spec = GetTargetSpecies(bees);
            return GetSpeciePartners(bees, bee,
                spec.Contains(bee.SpecieChromosome.Primary.Value)
                    ? bee.SpecieChromosome.Primary.Value
                    : bee.SpecieChromosome.Secondary.Value);
        }
        private List<BeeStack> GetPossiblePartners(BeePool bees, Bee bee)
        {
            var partners = bee.Gender == Gender.Princess ? bees.Drones : bees.Princesses;

            void ExcludeParetoEqualWithDifferentSpecies()
            {
                var specie1 = (SpecieChromosome)bee[BeeGeneticDatabase.StatNames.Specie];
                for (int i = 0; i < partners.Count; i++)
                {
                    var partner = partners[i];

                    var specie2 = (SpecieChromosome)partner.Bee[BeeGeneticDatabase.StatNames.Specie];
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
