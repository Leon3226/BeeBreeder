using System;

namespace BeeBreeder.Common.Model.Genetics
{
    public interface IChromosome<T>  where T: struct
    {
        IGene<T> Primary { get; set; }
        IGene<T> Secondary { get; set; }
        bool Clean { get; }
        T ResultantAttribute { get; }

    }
    
    public interface IChromosome : ICrossable
    {
        IGene Primary { get; }
        bool Clean { get; }
        IGene Secondary { get; }
        object ResultantAttribute { get; }

    }
}