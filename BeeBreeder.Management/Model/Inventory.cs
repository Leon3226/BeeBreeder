using BeeBreeder.WebAPI.Serializing.JSONConverters;
using System.Text.Json.Serialization;

namespace BeeBreeder.Management.Model
{
    public class GameInventory
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringToIntConverter))]
        public int Size { get; set; }

        public Item[] Items { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
