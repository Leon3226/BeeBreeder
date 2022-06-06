using BeeBreeder.WebAPI.Serializing.JSONConverters;
using System.Text.Json.Serialization;

namespace BeeBreeder.Management.Model
{
    public class Inventory
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringToIntConverter))]
        public int Size { get; set; }
    }
}
