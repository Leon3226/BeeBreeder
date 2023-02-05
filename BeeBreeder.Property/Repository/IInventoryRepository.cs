using BeeBreeder.Property.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Property.Repository
{
    public interface IInventoryRepository
    {
        Task<Inventory[]> GetInventoriesAsync(string transposerId);
        Task<Inventory> GetInventoryAsync(string transposerId, int side);
        Task AddInventoryAsync(string transposerId, int side, Inventory inventory);
        Task UpdateInventoryAsync(string transposerId, int side, Inventory inventory);
        Task DeleteInventoryAsync(string transposerId, int side);
    }
}
