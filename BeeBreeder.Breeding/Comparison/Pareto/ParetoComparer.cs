using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Worth;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.Comparison.Pareto
{
    public class ParetoComparer : IParetoComparer
    {
        public Bee ParetoBetter(Bee first, Bee second, BreedingTarget target = null)
        {
            bool firstHasBest = false;
            bool secondHasBest = false;

            foreach (var firstChromosome in first.Genotype.Chromosomes)
            {
                var secondChromosome = second[firstChromosome.Key];
                var best = ParetoBetter(firstChromosome.Value, secondChromosome, target: target);
                if (best == firstChromosome.Value)
                    firstHasBest = true;
                if (best == secondChromosome)
                    secondHasBest = true;
                if (firstHasBest && secondHasBest)
                    return null;
            }

            if (firstHasBest)
                return first;

            if (secondHasBest)
                return second;

            return null;
        }

        public List<Bee> ParetoOptimal(IEnumerable<Bee> bees, BreedingTarget target = null)
        {
            var toCheck = bees.ToList();
            var set = new HashSet<Bee>(toCheck.Count / 2);
            for (int i = 0; i < toCheck.Count; i++)
            {
                var bee1 = toCheck[i];
                if (set.Contains(bee1))
                    continue;
                for (int j = i + 1; j < toCheck.Count; j++)
                {
                    var bee2 = toCheck[j];
                    if (set.Contains(bee2))
                        continue;
                    if (bee1 == bee2) continue;
                    var better = ParetoBetter(bee1, bee2, target);
                    if (better != null)
                    {
                        var worse = better == bee1 ? bee2 : bee1;
                        set.Add(worse);
                        if (worse == bee1)
                        {
                            //i--;
                            break;
                        }
                    }
                }
            }

            return toCheck.Except(set).ToList();
        }

        public async Task<List<Bee>> ParetoOptimalAsync(IEnumerable<Bee> bees, BreedingTarget target = null)
        {
            var toCheck = bees.ToList();
            var removed = new HashSet<Bee>(toCheck.Count / 2);

            async Task GetToRemoveEnumerator(Bee bee1, IEnumerator<Bee> bees)
            {
                while (bees.MoveNext())
                {
                    if (removed.Contains(bee1))
                        break;
                    var bee2 = bees.Current;
                    if (bee1 == bee2) continue;
                    if (removed.Contains(bee2))
                        continue;
                    var better = await Task.Run(() => ParetoBetter(bee1, bee2, target));
                    if (better != null)
                    {
                        var worse = better == bee1 ? bee2 : bee1;
                        removed.Add(worse);
                        if (worse == bee1)
                            break;
                    }
                }
            }

            for (int i = 0; i < toCheck.Count - 1; i++)
            {
                var bee1 = toCheck[i];

                await Task.Run(async () =>
                {
                    using var enumerator = toCheck.Skip(i + 1).GetEnumerator();
                    await GetToRemoveEnumerator(bee1, enumerator);
                });
            }

            return toCheck.Except(removed).ToList();
        }
        public List<BeeStack> ParetoOptimal(IEnumerable<BeeStack> bees, BreedingTarget target = null)
        {
            var optimal = ParetoOptimal(bees.Select(x => x.Bee), target);
            return bees.Where(x => optimal.Contains(x.Bee)).ToList();
        }

        public async Task<List<BeeStack>> ParetoOptimalAsync(IEnumerable<BeeStack> bees,
            BreedingTarget target = null)
        {
            var toCheck = bees.ToList();
            var optimal = await Task.Run(() => ParetoOptimal(toCheck.Select(x => x.Bee), target));
            return toCheck.Where(x => optimal.Contains(x.Bee)).ToList();
        }

        public IChromosome ParetoBetter(IChromosome first, IChromosome second, bool compareOrder = false, BreedingTarget target = null)
        {
            //TODO: Can be optimized 
            var fpp = first.Primary.Compare(second.Primary, first.Property, target: target);
            var fps = first.Primary.Compare(second.Secondary, first.Property, target: target);
            var fsp = first.Secondary.Compare(second.Primary, first.Property, target: target);
            var fss = first.Secondary.Compare(second.Secondary, first.Property, target: target);

            List<ProbabilityUtils.Model.Worth.Comparison> comparisons = new List<ProbabilityUtils.Model.Worth.Comparison>() { fpp, fps, fsp, fss };

            if (comparisons.Any(x => x == ProbabilityUtils.Model.Worth.Comparison.Better))
            {
                if (comparisons.Any(x => x == ProbabilityUtils.Model.Worth.Comparison.Worse))
                {
                    return null;
                }

                return first;
            }
            else
            {
                if (comparisons.All(x => x == ProbabilityUtils.Model.Worth.Comparison.Equal))
                    return null;
            }

            return second;
        } }
}
