using System.Collections.Generic;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Bees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BeeBreeder.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiaryDataController : ControllerBase
    {
        private readonly ILogger<BreederController> _logger;
        private readonly IBreedingSimulator _simulator;
        private readonly IPositionsController _positionsController;

        public ApiaryDataController(ILogger<BreederController> logger, IBreedingSimulator simulator, IPositionsController positionsController)
        {
            _logger = logger;
            _simulator = simulator;
            _positionsController = positionsController;
        }

        [HttpGet]
        public async Task<BeePool> AvaliableBeesAsync()
        {
            var generator = new BeeGenerator();
            var pool = new BeePool
            {
                Bees = new List<BeeStack>
                {
                    new(generator.Generate(Species.Forest, Gender.Princess), 8),
                    new(generator.Generate(Species.Forest), 8),
                    new(generator.Generate(Species.Meadows, Gender.Princess), 8),
                    new(generator.Generate(Species.Meadows), 8),
                    new(generator.Generate(Species.Steadfast), 1),
                    new(generator.Generate(Species.Tropical), 1),
                    new(generator.Generate(Species.Modest), 1),
                    new(generator.Generate(Species.Modest, Gender.Princess),  1),
                    new(generator.Generate(Species.Tropical, Gender.Princess),  1)
                }
            };

            _simulator.Pool = pool;
            _simulator.Breed(3000);

            return pool;
        }
    }
}
