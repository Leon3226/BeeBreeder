using System;
using System.Text.RegularExpressions;

namespace BeeBreeder.Common.Model.Genetics
{
    public struct Gene<T> : IGene<T> where T: struct
    {
        public Type Type => typeof(T);

        public T Value { get; init; }

        public Gene(T value)
        {
            Dominant = false;
            Value = value;
        }

        object IGene.Value => Value;

        public bool Dominant { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}