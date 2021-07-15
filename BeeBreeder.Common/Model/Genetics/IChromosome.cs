using System;

namespace BeeBreeder.Common.Model.Genetics
{
    public interface IChromosome<T>  where T: struct
    {
        IGene<T> Primary { get; set; }
        IGene<T> Secondary { get; set; }
        T ResultantAttribute { get; }

    }
    
    public interface IChromosome : ICrossable
    {
        IGene Primary { get; }
        IGene Secondary { get; }
    }
}