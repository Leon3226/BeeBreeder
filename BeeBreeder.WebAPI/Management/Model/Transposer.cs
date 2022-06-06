namespace BeeBreeder.WebAPI.Management.Model
{
    public class Transposer
    {
        public string Address { get; set; }

        public Inventory[] Inventories { get; } = new Inventory[6];
    }
}
