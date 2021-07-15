using System.Collections.Generic;
using BeeBreeder.Common.Model.Genetics;

namespace BeeBreeder.Breeding.ProbabilityUtils.Model.Chance
{
    public interface IChromosomeCrossChance
    {
        IChromosome First { get; }
        IChromosome Second { get; }
        void ClearMutations();

        IReadOnlyCollection<Chance<IChromosome>> Chances { get; }
    }

    public interface IChromosomeCrossChance<T> : IChromosomeCrossChance where T : struct 
    {
        Chromosome<T> First { get; set; }
        Chromosome<T> Second { get; set; }
        IReadOnlyCollection<(Chromosome<T>, double)> Mutations { get; }
        IReadOnlyCollection<Chance<Chromosome<T>>> Chances { get; }
    }
}