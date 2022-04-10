using System;

namespace BeeBreeder.Common.Model.Genetics
{
    public interface IChromosome<T> : IChromosome  where T: struct
    {
        IGene<T> Primary { get; set; }
        IGene<T> Secondary { get; set; }
        bool Clean { get; }
        T ResultantAttribute { get; }

    }
    
    public interface IChromosome : ICrossable
    {
        public string Property { get; set; }
        IGene Primary { get; }
        bool Clean { get; }
        IGene Secondary { get; }
        object ResultantAttribute { get; }

    }
}