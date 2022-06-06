using BeeBreeder.Management.Repository;
using BeeBreeder.WebAPI.Management.Model;
using BeeBreeder.WebAPI.Model;
using BeeBreeder.WebAPI.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeBreeder.WebAPI.Management
{
    public class ApiaryManager
    {
        public string ApiaryId { get; set; }
        public string ApiaryName { get; set; }

        public Transposer[] Trasposers;

        private readonly IGameApiariesDataRepository _gameApiariesDataRepository;

        public ApiaryManager(IGameApiariesDataRepository gameApiariesDataRepository)
        {
            _gameApiariesDataRepository = gameApiariesDataRepository;
        }

        public async Task LoadData()
        {
            Trasposers = (await _gameApiariesDataRepository.TransposersAsync(ApiaryName)).Select(x => new Transposer { Address = x}).ToArray();
            foreach (var transposer in Trasposers)
            {
                var sides = await _gameApiariesDataRepository.InventoriesAsync(ApiaryName, transposer.Address);
                for (int i = 0; i < sides.Length; i++)
                {
                    var inventory = sides[i];
                    if (inventory == null)
                        continue;
                    transposer.Inventories[i] = new Inventory { Name = sides[i].Name, Size = sides[i].Size };
                    transposer.Inventories[i].Items = await _gameApiariesDataRepository.ItemsAsync(ApiaryName, transposer.Address, i);
                }
            }
        }
    }
}
