using System;
using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Breeding.Targeter;
using BeeBreeder.Common;
using BeeBreeder.Common.Data;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Data;
using BeeBreeder.Common.Model.Extensions;
using BeeBreeder.Common.Model.Genetics;
using BeeBreeder.Common.Model.Genetics.Phenotype;

namespace BeeBreeder.Breeding.Analyzer
{
    public class ExtendedNaturalSelectionAnalyzer : IBreedAnalyzer
    {
        private readonly MutationTree _mutationTree;
        private readonly ISpecieTargeter _specieTargeter;

        public int PureMinCount = 5;
        public int ImpureMinCount = 10;

        public ExtendedNaturalSelectionAnalyzer(ISpecieTargeter specieTargeter, MutationTree mutationTree)
        {
            _specieTargeter = specieTargeter;
            _mutationTree = mutationTree;
        }

        public List<(Bee Princess, Bee Drone)> GetBreedingPairs(BeePool bees, int count = 0)
        {
            List<(Bee, Bee)> toReturn = new List<(Bee, Bee)>();

            if (count < 0)
                return toReturn;

            var princesses = bees.Princesses.Copy();
            var drones = bees.Drones.Copy();
            if (!princesses.Any() || !drones.Any())
                return toReturn;

            _specieTargeter.Bees = bees;
            var neededSpecies = _specieTargeter.CalculatedTargets.ToList();
            var toPreserve = princesses.Where(x => neededSpecies.Contains(x.Bee.SpecieChromosome.Primary.Value) || neededSpecies.Contains(x.Bee.SpecieChromosome.Secondary.Value)).ToList();
            var toPreservePure = toPreserve.Where(x => x.Bee.SpecieChromosome.Clean).ToList();
            var toPreserveImpure = toPreserve.Except(toPreservePure).ToList();

            InsertPairs(toPreservePure, GetPreserveImportantPartnersPure);
            InsertPairs(toPreserveImpure, GetPreserveImportantPartnersImpure);
            InsertPairs(princesses.ToList(), GetPossiblePartners);

            void InsertPairs(List<BeeStack> breedingPrincesses, Func<BeePool, Bee, List<BeeStack>> partnerFilter)
            {
                var overallCount = breedingPrincesses.OverallCount();
                for (int i = 0; i < overallCount; i++)
                {
                    breedingPrincesses = breedingPrincesses.Where(x => x.Count > 0).ToList();
                    if (breedingPrincesses.Count == 0 || drones.Count == 0)
                        return;

                    var princess = breedingPrincesses[RandomGenerator.GenerateInt(0, breedingPrincesses.Count)];
                    var partners = partnerFilter(new BeePool(drones), princess.Bee);

                    if (partners.Count == 0) continue;
                    var drone = partners[RandomGenerator.GenerateInt(0, partners.Count)];

                    princesses.RemoveCount(princess, 1);
                    drones.RemoveCount(drone, 1);

                    toReturn.Add((princess.Bee, drone.Bee));
                }
            }

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
            _specieTargeter.Bees = bees;
            var spec = _specieTargeter.CalculatedTargets.ToList();
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
                var specie1 = (Chromosome<Species>)bee[Constants.StatNames.Specie];
                for (int i = 0; i < partners.Count; i++)
                {
                    var partner = partners[i];

                    var specie2 = (Chromosome<Species>)partner.Bee[Constants.StatNames.Specie];
                    if (((specie1.Clean &&
                          specie2.Clean) ||
                         !(specie1.Primary.Value == specie2.Primary.Value &&
                           specie1.Secondary.Value == specie2.Secondary.Value &&
                           specie1.Primary.Value == specie2.Primary.Value)) &&
                        !_mutationTree.PossibleResults(new List<Species>()
                            {specie1.Primary.Value, specie2.Primary.Value}).Any() &&
                        !_mutationTree.PossibleResults(new List<Species>()
                            {specie1.Primary.Value, specie2.Secondary.Value}).Any() &&
                        !_mutationTree.PossibleResults(new List<Species>()
                            {specie1.Secondary.Value, specie2.Secondary.Value}).Any()
                    )
                    {
                        var isEqual = true;
                        foreach (var gene in bee.Genotype.Chromosomes)
                        {
                            if (gene.Key == Constants.StatNames.Specie)
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
