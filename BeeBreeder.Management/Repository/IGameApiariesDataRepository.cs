using BeeBreeder.Management.Model;

namespace BeeBreeder.Management.Repository
{
    public interface IGameApiariesDataRepository
    {
        Task<Inventory[]> InventoriesAsync(string apiary, string transposer);
        bool IsActive(string apiary);
        Task<Item[]> ItemsAsync(string apiary, string transposer, int side);
        Task<int> MoveAsync(string apiary, string transposer, MoveRequest move);
        Task PrintAsync(string apiary, string message);
        Task<string[]> TransposersAsync(string apiary);
    }
}