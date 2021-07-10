namespace BeeBreeder.Common.Model.Genetics
{
    public interface IGene
    {
        bool Dominant { get; set; }
    }
    
    public interface IGene<T> : IGene
    {
        T Property { get; set; }
    }
}