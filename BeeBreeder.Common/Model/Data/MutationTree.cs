using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BeeBreeder.Common.AlleleDatabase.Bee;

namespace BeeBreeder.Common.Model.Data
{
    public class MutationTree
    {
        public List<MutationNode> Nodes;
        public List<MutationLink> Links;

        public MutationNode this[Species specie] => Nodes.FirstOrDefault(x => x.Specie == specie);

        public List<Species> PossibleResults(List<Species> existingSpecies)
        {
            List<Species> possible = existingSpecies.ToList();
            List<Species> toAdd = new List<Species>();

            do
            {
                toAdd.Clear();
                toAdd = Links.Where(x => possible.Contains(x.Parent1.Specie) && possible.Contains(x.Parent2.Specie))
                    .Select(x => x.Child.Specie).ToList();
                toAdd = toAdd.Except(possible).ToList();
                possible.AddRange(toAdd);
            } while (toAdd.Count != 0);

            return possible.Except(existingSpecies).ToList();
        }

        public bool IsEssentialForGetting(List<Species> targets, List<Species> existing, Species inspectable)
        {
            var include = existing.ToList();
            include.Add(inspectable);
            var except = existing.Except(new[] {inspectable}).ToList();
            var possibleWithout = PossibleResults(except);
            var possibleWith = PossibleResults(include);
            var isPossibleWith = targets.All(x => possibleWith.Contains(x));
            var isPossibleWithout = targets.All(x => possibleWithout.Contains(x));
            return isPossibleWith && !isPossibleWithout;
        }

        public static MutationTree FromSpecieCombinations(List<SpecieCombination> specieCombinations)
        {
            var tree = new MutationTree();
            if (specieCombinations == null)
                return null;

            var parents1 = specieCombinations.Select(x => x.Parent1);
            var parents2 = specieCombinations.Select(x => x.Parent2);
            var child = specieCombinations.Select(x => x.MutationResult);

            var all = parents1.Concat(parents2).Concat(child).Distinct().ToList();

            tree.Nodes = all.Select(x => new MutationNode() {Specie = x}).ToList();

            tree.Links = specieCombinations.Select(x =>
            {
                var parent1 = tree.Nodes.FirstOrDefault(y => y.Specie == x.Parent1);
                var parent2 = tree.Nodes.FirstOrDefault(y => y.Specie == x.Parent2);
                var childNode = tree.Nodes.FirstOrDefault(y => y.Specie == x.MutationResult);

                var link = new MutationLink()
                {
                    Parent1 = parent1,
                    Parent2 = parent2,
                    Child = childNode,
                    MutationChance = x.MutationChance
                };

                parent1?.Child.Add(link);
                parent2?.Child.Add(link);
                childNode?.Parents.Add(link);

                return link;
            }).ToList();

            return tree;
        }

        public List<Species> OnlyNecessaryForGettingIfPossible(List<Species> targets, List<Species> existing)
        {
            List<Species> necessary = new List<Species>();

            MutationNode RecursiveNecessary(MutationNode current)
            {
                var contains = existing.Contains(current.Specie);
                if (contains)
                {
                    return current;
                }
                else
                {
                    var parents = current.Parents.FirstOrDefault();
                    if (parents != null)
                    {
                        var first = RecursiveNecessary(parents.Parent1);
                        var second = RecursiveNecessary(parents.Parent2);
                        if (first != null && second != null)
                        {
                            necessary.Add(first.Specie);
                            necessary.Add(second.Specie);
                        }
                    }
                }

                return null;
            }

            foreach (var target in targets)
            {
                if (existing.Contains(target))
                    continue;

                var node = Nodes.FirstOrDefault(x => x.Specie == target);
                RecursiveNecessary(node);
            }

            var result = necessary.Distinct().ToList();
            return result;
        }

        public List<Species> OnlyNecessaryForGettingIfPossibleAndHaveEnough(List<Species> targets,
            Dictionary<Species, int> existing, int threshold = 20)
        {
            List<Species> necessary = new List<Species>();

            MutationNode RecursiveNecessary(MutationNode current)
            {
                var exists = false;
                if (existing.TryGetValue(current.Specie, out int count))
                {
                    if (count >= threshold)
                    {
                        return current;
                    }
                    else
                    {
                        exists = true;
                    }
                }

                var parents = current.Parents;
                if (parents != null)
                {
                    foreach (var pair in parents)
                    {
                        var first = RecursiveNecessary(pair.Parent1);
                        var second = RecursiveNecessary(pair.Parent2);
                        if (first != null && second != null)
                        {
                            necessary.Add(first.Specie);
                            necessary.Add(second.Specie);
                            if (exists)
                                necessary.Add(current.Specie);
                        }
                    }
                }

                if (exists)
                    return current;

                return null;
            }

            foreach (var target in targets)
            {
                if (existing.Any(x => x.Key == target && x.Value >= threshold))
                    continue;

                var node = Nodes.FirstOrDefault(x => x.Specie == target);
                RecursiveNecessary(node);
            }

            var result = necessary.Distinct().ToList();
            return result;
        }
    }
}