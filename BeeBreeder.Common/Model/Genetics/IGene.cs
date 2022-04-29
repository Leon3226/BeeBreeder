using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace BeeBreeder.Common.Model.Genetics
{
    public interface IGene
    {
        [XmlIgnore]
        [JsonIgnore]
        Type Type { get; }
        object Value { get; }
        bool Dominant { get; set; }
    }
    
    public interface IGene<out T> : IGene
    {
        new T Value { get; }
    }
}