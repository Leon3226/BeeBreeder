using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics.Phenotype;
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
    [Route("[controller]")]
    [ApiController]
    public class ApiaryDataController : ControllerBase
    {
        private readonly ILogger<ApiaryDataController> _logger;
        private readonly IBreedingSimulator _simulator;
        private readonly IPositionsController _positionsController;
        private readonly BeeGenerator _beeGenerator;
        private readonly IGameApiariesDataRepository _gameApiariesDataRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IApiaryRepository _apiaryDataRepository;

        public ApiaryDataController(ILogger<ApiaryDataController> logger, 
            IBreedingSimulator simulator, 
            IPositionsController positionsController,
            BeeGenerator beeGenerator,
            IGameApiariesDataRepository gameApiariesDataRepository, 
            UserManager<IdentityUser> userManager, 
            IApiaryRepository apiaryDataRepository)
        {
            _logger = logger;
            _simulator = simulator;
            _positionsController = positionsController;
            _beeGenerator = beeGenerator;
            _gameApiariesDataRepository = gameApiariesDataRepository;
            _userManager = userManager;
            _apiaryDataRepository = apiaryDataRepository;
        }

        [HttpGet]
        public async Task<BeePool> AvaliableBeesAsync(int iterations = 1000)
        {
            var pool = new BeePool
            {
                Bees = new List<BeeStack>
                {
                    new(_beeGenerator.Generate("Forest", Gender.Princess), 8),
                    new(_beeGenerator.Generate("Forest"), 8),
                    new(_beeGenerator.Generate("Meadows", Gender.Princess), 8),
                    new(_beeGenerator.Generate("Meadows"), 8),
                    new(_beeGenerator.Generate("Steadfast"), 1),
                    new(_beeGenerator.Generate("Tropical"), 1),
                    new(_beeGenerator.Generate("Modest"), 1),
                    new(_beeGenerator.Generate("Modest", Gender.Princess),  1),
                    new(_beeGenerator.Generate("Tropical", Gender.Princess),  1)
                }
            };

            Stopwatch sw = new Stopwatch();
            sw.Start();

            _simulator.Pool = pool;
            await Task.Run(() => _simulator.Breed(iterations));

            sw.Stop();
            _logger.Log(LogLevel.Debug, $"Breeded {iterations} breeds in {sw.Elapsed}");

            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)HttpContext.User.Claims);
            return pool;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<Apiary[]>> Apiaries()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            var apiaries = await _apiaryDataRepository.GetApiariesAsync(userId);
            return apiaries;
        }

        [Route("{apiaryComputer}")]
        [HttpGet]
        public async Task<string[]> Transposers(string apiaryComputer)
        {
            return await _gameApiariesDataRepository.TransposersAsync(apiaryComputer);
        }

        [Route("{apiaryComputer}/{transposer}")]
        [HttpGet]
        public async Task<Inventory[]> Inventories(string apiaryComputer, string transposer)
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
