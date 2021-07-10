using System;

namespace BeeBreeder.Common.Model.Genetics
{
    public interface IChromosome<T> 
    {
        string Property { get; set; }
        IGene<T> Gene1 { get; set; }
        IGene<T> Gene2 { get; set; }
        Chromosome<T> Cross(Chromosome<T> secondPair, Random random = null);
    }
}