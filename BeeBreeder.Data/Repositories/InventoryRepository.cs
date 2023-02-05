using BeeBreeder.Property.Model;
using BeeBreeder.Property.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Context = BeeBreeder.Data.Models.BeeBreederContext;

namespace BeeBreeder.Data.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        public async Task AddInventoryAsync(string transposerId, int side, Inventory inventory)
        {
            using (var context = new Context())
            {
                await context.AddAsync(new Models.Inventory
                {
                    Side = side,
                    TransposerId = transposerId,
                    Description = inventory.Description,
                    InGameId = inventory.InGameId,
                    InGameLabel = inventory.InGameLabel,
                    ItemUnderId = inventory.ItemUnderId,
                    Name = inventory.Name
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteInventoryAsync(string transposerId, int side)
        {
            using (var context = new Context())
            {
                var dbInventory = context.Inventories.Single(x => x.TransposerId == transposerId && x.Side == side);
                if (dbInventory != null)
                {
                    context.Inventories.Remove(dbInventory);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<Inventory> GetInventoryAsync(string transposerId, int side)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.Inventories.Where(x => x.TransposerId == transposerId && x.Side == side).Select(x => new Inventory()
                    {
                        Side = x.Side,
                        TransposerId = x.TransposerId,
                        Description = x.Description,
                        InGameId = x.InGameId,
                        InGameLabel = x.InGameLabel,
                        ItemUnderId = x.ItemUnderId,
                        Name = x.Name
                    }).SingleOrDefault();
                });
            }
        }

        public async Task<Inventory[]> GetInventoriesAsync(string transposerId)
        {
            using (var context = new Context())
            {
                return await Task.Run(() =>
                {
                    return context.Inventories.Where(x => x.TransposerId == transposerId).Select(x => new Inventory()
                    {
                        Side = x.Side,
                        TransposerId = x.TransposerId,
                        Description = x.Description,
                        InGameId = x.InGameId,
                        InGameLabel = x.InGameLabel,
                        ItemUnderId = x.ItemUnderId,
                        Name = x.Name
                    }).ToArray();
                });
            }
        }

        public async Task UpdateInventoryAsync(string transposerId, int side, Inventory inventory)
        {
            using (var context = new Context())
            {
                var dbInventory = context.Inventories.SingleOrDefault(x => x.TransposerId == transposerId && x.Side == side);
                if (dbInventory != null)
                {
                    dbInventory.Description = inventory.Description;
                    dbInventory.InGameId = inventory.InGameId;
                    dbInventory.InGameLabel = inventory.InGameLabel;
                    dbInventory.ItemUnderId = inventory.ItemUnderId;
                    dbInventory.Name = inventory.Name;
                }
                await context.SaveChangesAsync();
            }
        }


    }
}
