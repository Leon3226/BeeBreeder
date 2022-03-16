using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BeeBreeder.Common.Model.Genetics
{
    public struct Gene<T> : IGene<T> where T: struct
    {
        [XmlIgnore]
        [JsonIgnore]
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