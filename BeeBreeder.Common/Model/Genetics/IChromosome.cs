namespace BeeBreeder.Common.Model.Genetics
{
    public interface IChromosome<T> : IChromosome  where T: struct
    {
        new IGene<T> Primary { get; set; }
        new IGene<T> Secondary { get; set; }
        new bool Clean { get; }
        new T ResultantAttribute { get; }

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