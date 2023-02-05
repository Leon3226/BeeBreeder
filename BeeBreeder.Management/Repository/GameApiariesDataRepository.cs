using BeeBreeder.Management.Model;
using BeeBreeder.Management.Parser;
using BeeBreeder.Management.Sockets;
using System.Threading.Tasks;

namespace BeeBreeder.Management.Repository
{
    public class GameApiariesDataRepository : IGameApiariesDataRepository
    {
        private readonly ApiariesSocketsManager _socketManager;
        private readonly IGameApiaryRequestParser _apiaryRequestConverter;

        private const string transposersRequest = "transposers";
        private const string inventoriesRequest = "inventories";
        private const string itemsRequest = "items";
        private const string moveRequest = "move";
        private const string printRequest = "print";

        public GameApiariesDataRepository(ApiariesSocketsManager socketManager, IGameApiaryRequestParser apiaryRequestConverter)
        {
            _socketManager = socketManager;
            _apiaryRequestConverter = apiaryRequestConverter;
        }

        public bool IsActive(string apiary)
        {
            return _socketManager.IsActive(apiary);
        }

        public async Task<string[]> TransposersAsync(string apiary)
        {
            var raw = await _socketManager.RequestToApiaryAsync(apiary, transposersRequest);
            return _apiaryRequestConverter.ToTransposers(raw);
        }

        public async Task<GameInventory[]> InventoriesAsync(string apiary, string transposer)
        {
            var raw = await _socketManager.RequestToApiaryAsync(apiary, $"{inventoriesRequest} {transposer}");
            return _apiaryRequestConverter.ToInventories(raw);
        }

        public async Task<Item[]> ItemsAsync(string apiary, string transposer, int side)
        {
            var raw = await _socketManager.RequestToApiaryAsync(apiary, $"{itemsRequest} {transposer} {side}");
            return _apiaryRequestConverter.ToItems(raw);
        }

        public async Task<int> MoveAsync(string apiary, string transposer, MoveRequest move)
        {
            var raw = await _socketManager.RequestToApiaryAsync(apiary, $"{moveRequest} {transposer} {move.FirstSide} {move.FirstSlot} {move.SecondSide} {move.SecondSlot} {move.Amount}");
            return _apiaryRequestConverter.ToInt(raw);
        }

        public async Task PrintAsync(string apiary, string message)
        {
            await _socketManager.RequestToApiaryAsync(apiary, $"{printRequest} {message}");
        }
    }
}
