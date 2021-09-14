using System;

namespace BeeBreeder.Common.Model.Genetics
{
    public interface IGene
    {
        Type Type { get; }
        object Value { get; }
        bool Dominant { get; set; }
    }
    
    public interface IGene<out T> : IGene where T : struct
    {
        T Value { get; }
    }
}