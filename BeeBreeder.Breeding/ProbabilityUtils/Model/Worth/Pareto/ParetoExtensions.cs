using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeeBreeder.Common.Model.Bees;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Worth.Pareto
{
    public class BeeComparator : IComparer<Bee>
    {
        private BreedingTarget _target;

        public BeeComparator(BreedingTarget target)
        {
            _target = target;
        }

        public int Compare(Bee x, Bee y)
        {
            ParetoExtensions.Comparisons++;
            var res = x.ParetoBetter(y, _target);
            if (res == null)
                return 0;
            if (res == x)
                return 1;
            return -1;
        }
    }

    public static class ParetoExtensions
    {
        public static int Comparisons = 0;

        public static Bee ParetoBetter(this Bee first, Bee second, BreedingTarget target = null)
        {
            bool firstHasBest = false;
            bool secondHasBest = false;

            //var genesList = first.Genotype.Genes.ToArray();
            //for (int i = 0; i < genesList.Length; i++)
            //{
            //    var firstChromosome = genesList[i];
            //    var secondChromosome = second[firstChromosome.Key];
            //    var best = firstChromosome.Value.ParetoBetter(secondChromosome, target: target);
            //    if (best == firstChromosome.Value)
            //        firstHasBest = true;
            //    if (best == secondChromosome)
            //        secondHasBest = true;
            //    if (firstHasBest && secondHasBest)
            //        return null;
            //}

            foreach (var firstChromosome in first.Genotype.Genes)
            {
                var secondChromosome = second[firstChromosome.Key];
                var best = firstChromosome.Value.ParetoBetter(secondChromosome, target: target);
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

        public static List<Bee> ParetoOptimal(this IEnumerable<Bee> bees, BreedingTarget target = null)
        {
            var toCheck = bees.ToList();
            //var comparer = new BeeComparator(target);
            //var sorted = toCheck.OrderByDescending(x => x, comparer).ToList();
            //var a = sorted.TakeWhile((x, i) =>
            //{
            //    if (i == 0)
            //        return true;
            //    return ParetoBetter(x, sorted[i - 1]) == null;
            //}).ToList();
            //return a;
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
                    Comparisons++;
                    var better = ParetoBetter(bee1, bee2, target);
                    if (better != null)
                    {
                        var worse = better == bee1 ? bee2 : bee1;
                        set.Add(worse);
                        //toCheck.Remove(worse);
                        if (worse == bee1)
                        {
                            //i--;
                            break;
                        }
                        //else j--;
                    }
                }
            }

            return toCheck.Except(set).ToList();
        }

        public static async Task<List<Bee>> ParetoOptimalAsync(this IEnumerable<Bee> bees, BreedingTarget target = null)
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
                    var better = await Task.Run(() =>
                    {
                        Comparisons++;
                        return ParetoBetter(bee1, bee2, target);
                    });
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
                async Task GetToRemove(Bee bee1, IEnumerable<Bee> bees)
                {
                    foreach (var bee2 in bees)
                    {
                        if (bee1 == bee2) continue;
                        var better = await Task.Run(() =>
                        {
                            Comparisons++;
                            return ParetoBetter(bee1, bee2, target);
                        });
                        if (better != null)
                        {
                            var worse = better == bee1 ? bee2 : bee1;
                            removed.Add(worse);
                        }
                    }
                }

                var bee1 = toCheck[i];
                //var beesToCompare = toCheck.Skip(i + 1).ToList();

                //await foreach (var bee in GetToRemove(bee1, beesToCompare))
                //{
                //    toCheck.Remove(bee);
                //}

                await Task.Run(async () =>
                {
                    using var enumerator = toCheck.Skip(i + 1).GetEnumerator();
                    await GetToRemoveEnumerator(bee1, enumerator);
                });
            }

            return toCheck.Except(removed).ToList();
        }
        public static List<BeeStack> ParetoOptimal(this IEnumerable<BeeStack> bees, BreedingTarget target = null)
        {
            var optimal = ParetoOptimal(bees.Select(x => x.Bee), target);
            return bees.Where(x => optimal.Contains(x.Bee)).ToList();
        }

        public static async Task<List<BeeStack>> ParetoOptimalAsync(this IEnumerable<BeeStack> bees,
            BreedingTarget target = null)
        {
            var toCheck = bees.ToList();
            var optimal = ParetoOptimal(toCheck.Select(x => x.Bee), target);
            return toCheck.Where(x => optimal.Contains(x.Bee)).ToList();
        }
    }
}