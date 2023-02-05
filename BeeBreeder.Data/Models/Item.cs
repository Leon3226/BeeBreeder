using System;
using System.Collections.Generic;

namespace BeeBreeder.Data.Models
{
    public partial class Item
    {
        public Item()
        {
            Inventories = new HashSet<Inventory>();
            ItemDropChances = new HashSet<ItemDropChance>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<ItemDropChance> ItemDropChances { get; set; }
    }
}
