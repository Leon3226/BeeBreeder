using System;
using Newtonsoft.Json;

namespace BeeBreeder.Common.Model.Genetics
{
    public struct Gene<T> : IGene<T>
    {
        [JsonIgnore]
        public Type Type => typeof(T);

        public T Value { get; init; }

        public Gene(T value)
        {
            Dominant = false;
            Value = value;
        }

        object IGene.Value => Value;

        public string StringValue => Value.ToString();

        public bool Dominant { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}