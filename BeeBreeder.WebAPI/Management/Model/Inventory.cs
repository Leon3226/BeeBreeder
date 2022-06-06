using BeeBreeder.Management.Model;

namespace BeeBreeder.WebAPI.Management.Model
{
    public class Inventory
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public Item[] Items { get; set; }
    }
}
