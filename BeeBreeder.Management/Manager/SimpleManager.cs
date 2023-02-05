using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Positioning;
using BeeBreeder.Management.Model;
using BeeBreeder.Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Management.Manager
{
    public class SimpleManager
    {
        public string[] ComputerNames { get; set; }

        public Computer[] Computers;

        private readonly IGameApiariesDataRepository _gameApiariesDataRepository;
        private readonly IPositionsController _positionsController;
        private readonly IBreedAnalyzer _breedAnalyzer;

        public SimpleManager(IGameApiariesDataRepository gameApiariesDataRepository,
            IPositionsController positionsController,
            IBreedAnalyzer breedAnalyzer)
        {
            _gameApiariesDataRepository = gameApiariesDataRepository;
            _positionsController = positionsController;
            _breedAnalyzer = breedAnalyzer;
        }

        public async Task LoadData(LoadDataParams loadDataParams = null)
        {
            loadDataParams = loadDataParams ?? new LoadDataParams();
            Computers = new Computer[ComputerNames.Length];
            for (int i = 0; i < ComputerNames.Length; i++)
            {
                if (loadDataParams.LoadComputers)
                {
                    Computers[i] = LoadComputer(ComputerNames[i], loadDataParams).Result;
                }
            }
        }

        public async Task PlaceBees()
        {
            var positions = new List<InventoryPosition>();
            var trasposersData = new List<TransposerData>();
            var bees = new Dictionary<Bee, InventoryPosition>();

            foreach (var computer in Computers)
            {
                foreach (var transposer in computer.Trasposers)
                {
                    trasposersData.Add(new TransposerData()
                    {
                        Biome = transposer.Biome,
                        Flowers = transposer.Flowers,
                        IsRoofed = transposer.Roofed,
                        Transposer = transposer.Adress
                    });
                    for (int i = 0; i < transposer.Inventories.Length; i++)
                    {
                        var inventory = transposer.Inventories[i];
                        if (inventory == null || inventory.Name != "forestry:apiary")
                            continue;

                        if (inventory.Items[0] != null || inventory.Items[1] != null)
                            continue;

                        var position = new InventoryPosition()
                        {
                            Side = i,
                            Trans = transposer.Adress
                        };
                        positions.Add(position);
                    }
                }
            }

            foreach (var computer in Computers)
            {
                foreach (var transposer in computer.Trasposers)
                {
                    for (int i = 0; i < transposer.Inventories.Length; i++)
                    {
                        var inventory = transposer.Inventories[i];
                        if (inventory == null)
                            continue;
                        //enderstorage:ender_storage

                        for (int j = 0; j < inventory.Items.Length; j++)
                        {
                            var item = inventory.Items[j];
                            var beeItem = item as BeeItem;

                            if (beeItem == null)
                                continue;

                            bees.Add(beeItem.BeeData.Bee, new InventoryPosition
                            {
                                Side = i,
                                Slot = j+1,
                                Trans = transposer.Adress
                            });
                        }

                    }
                }
            }

            var pairs = _breedAnalyzer.GetBreedingPairs(new BeePool(bees.Keys.ToList()));
            var assignedPositions = _positionsController.Assign(pairs, trasposersData, positions);
            var moves = assignedPositions.SelectMany(x =>
            new (InventoryPosition, InventoryPosition)[]
            {
                (bees[x.Item2.Princess],
                new InventoryPosition
                {
                    Side = x.position.Side,
                    Slot = 1,
                    Trans = x.position.Trans
                }),

                (bees[x.Item2.Drone],
                new InventoryPosition
                {
                    Side = x.position.Side,
                    Slot = 2,
                    Trans = x.position.Trans
                })
            });

            foreach (var move in moves)
            {
                MoveThroughEnderChest(move.Item1, move.Item2);
            }
        }

        public async Task MoveThroughEnderChest(InventoryPosition from, InventoryPosition to)
        {
            var firstEnderChest = Computers
                .SelectMany(x => x.Trasposers)?
                .SingleOrDefault(x => x.Adress == from.Trans)?
                .Inventories?
                .SingleOrDefault(x => x != null && x.Name == "enderstorage:ender_storage");

            var secondEnderChest = Computers
                .SelectMany(x => x.Trasposers)?
                .SingleOrDefault(x => x.Adress == to.Trans)?
                .Inventories?
                .SingleOrDefault(x => x != null && x.Name == "enderstorage:ender_storage");

            if (firstEnderChest == null || secondEnderChest == null)
                return;

            int? firstEmptySlot = null;
            for (int i = 0; i < firstEnderChest.Items.Length; i++)
            {
                if (firstEnderChest.Items[i] == null)
                {
                    firstEmptySlot = i+1;
                    break;
                }
            }

            if (!firstEmptySlot.HasValue)
                return;

            var firstComputer = Computers.SingleOrDefault(x => x.Trasposers.Select(trans => trans.Adress).Contains(from.Trans))?.Identifier;
            var secondComputer = Computers.SingleOrDefault(x => x.Trasposers.Select(trans => trans.Adress).Contains(to.Trans))?.Identifier;

            var firstRequest = new MoveRequest()
            {
                FirstSide = from.Side,
                SecondSide = 1, //Assuming ender chest is always on top
                FirstSlot = from.Slot,
                SecondSlot = firstEmptySlot.Value,
                Amount = 1
            };

            var secondRequest = new MoveRequest()
            {
                SecondSide = to.Side,
                FirstSide = 1, //Assuming ender chest is always on top
                SecondSlot = to.Slot,
                FirstSlot = firstEmptySlot.Value,
                Amount = 1
            };

            var movedIn = _gameApiariesDataRepository.MoveAsync(firstComputer, from.Trans, firstRequest).Result;
            if (movedIn > 0)
            {
                var movedOut = _gameApiariesDataRepository.MoveAsync(secondComputer, to.Trans, secondRequest).Result;
            }
        }

        public async Task<Computer> LoadComputer(string identifier, LoadDataParams loadDataParams = null)
        {
            loadDataParams = loadDataParams ?? new LoadDataParams();
            var computer = new Computer() { Identifier = identifier };
            if (loadDataParams.LoadTransposers)
            {
                computer.Trasposers = (_gameApiariesDataRepository.TransposersAsync(identifier).Result).Select(x => new Transposer { Adress = x }).ToArray();
                foreach (var transposer in computer.Trasposers)
                {
                    if (loadDataParams.LoadInventories)
                    {
                        var sides = _gameApiariesDataRepository.InventoriesAsync(identifier, transposer.Adress).Result;
                        if (loadDataParams.LoadItems)
                        {
                            for (int i = 0; i < sides.Length; i++)
                            {
                                var inventory = sides[i];
                                if (inventory == null)
                                    continue;
                                transposer.Inventories[i] = new GameInventory { Name = sides[i].Name, Size = sides[i].Size };
                                transposer.Inventories[i].Items = _gameApiariesDataRepository.ItemsAsync(identifier, transposer.Adress, i).Result;
                            }
                        }
                    }
                }
            }

            return computer;
        }
    }
}
