using System;
using System.Text.RegularExpressions;

namespace BeeBreeder.Common.Model.Genetics
{
    public struct Gene<T> : IGene<T> where T: struct
    {
        public T Value { get; init; }

        public bool Dominant { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}