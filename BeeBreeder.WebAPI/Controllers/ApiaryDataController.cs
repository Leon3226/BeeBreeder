using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Environment;
using BeeBreeder.Common.Model.Genetics.Phenotype;
using BeeBreeder.Management.Manager;
using BeeBreeder.Management.Model;
using BeeBreeder.Management.Repository;
using BeeBreeder.Property.Model;
using BeeBreeder.Property.Repository;
using BeeBreeder.WebAPI.Model;
using BeeBreeder.WebAPI.Sockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BeeBreeder.WebAPI.Controllers
{
    //TODO: Rewrite to match REST architecture
    [Route("api/[controller]")]
    [ApiController]
    public class InGameDataController : ControllerBase
    {
        //TODO: There is too much things here
        private readonly IComputerRepository _computerRepository;
        private readonly BeeGenerator _beeGenerator;
        private readonly IGameApiariesDataRepository _gameApiariesDataRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SimpleManager _manager;

        public InGameDataController(
            BeeGenerator beeGenerator,
            IGameApiariesDataRepository gameApiariesDataRepository, 
            UserManager<IdentityUser> userManager, 
            SimpleManager manager, 
            IComputerRepository computerRepository)
        {
            _beeGenerator = beeGenerator;
            _gameApiariesDataRepository = gameApiariesDataRepository;
            _userManager = userManager;
            _manager = manager;
            _computerRepository = computerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<BeePool>> AvaliableBeesAsync()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();

            return new BeePool
            {
                Bees = new List<BeeStack>
                {
                    new(_beeGenerator.Generate("Forest", Gender.Princess), 8),
                    new(_beeGenerator.Generate("Forest"), 8),
                    new(_beeGenerator.Generate("Meadows", Gender.Princess), 8),
                    new(_beeGenerator.Generate("Meadows"), 8),
                    new(_beeGenerator.Generate("Steadfast"), 1)
                }
            };

            _manager.ComputerNames = (await _computerRepository.GetComputersAsync(userId)).Select(x => x.Identifier).ToArray();
            await _manager.LoadData();

            var allBees = _manager.Computers
                .SelectMany(x => x?.Trasposers)
                .Where(x => x != null && x.Inventories != null)
                .SelectMany(x => x?.Inventories)
                .Where(x => x != null && x.Items != null)
                .SelectMany(x => x?.Items)
                .Where(x => x != null && x is BeeItem)
                .Cast<BeeItem>()
                .Select(x => x.BeeData)
                .ToList();
            var pool = new BeePool
            {
                Bees = allBees
            };

            return pool;
        }

        [Route("{apiaryComputer}")]
        [HttpGet]
        public async Task<string[]> Transposers(string apiaryComputer)
        {
            return await _gameApiariesDataRepository.TransposersAsync(apiaryComputer);
        }

        [Route("{apiaryComputer}/{transposer}")]
        [HttpGet]
        public async Task<GameInventory[]> Inventories(string apiaryComputer, string transposer)
        {
            return await _gameApiariesDataRepository.InventoriesAsync(apiaryComputer, transposer);
        }

        [Route("{apiaryComputer}/{transposer}/{side}")]
        [HttpGet]
        public async Task<Item[]> Items(string apiaryComputer, string transposer, int side)
        {
            return await _gameApiariesDataRepository.ItemsAsync(apiaryComputer, transposer, side);
        }

        //TODO: Not a REST! Needs revision
        [Route("{apiaryComputer}/{transposer}/move")]
        [HttpPost]
        public async Task<int> Move(string apiaryComputer, string transposer, [FromBody]MoveRequest moveRequest)
        {
            return await _gameApiariesDataRepository.MoveAsync(apiaryComputer, transposer, moveRequest);
        }

        [Route("{apiaryComputer}/messages")]
        [HttpPost]  
        public async Task Print(string apiaryComputer, [FromBody]ApiaryTextMessage message)
        {
            await _gameApiariesDataRepository.PrintAsync(apiaryComputer, message.Message);
        } 
    }
}   
