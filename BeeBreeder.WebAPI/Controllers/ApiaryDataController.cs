using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Generation;
using BeeBreeder.Breeding.Positioning;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Genetics.Phenotype;
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
        private readonly BeeGenerator _beeGenerator;

        public ApiaryDataController(ILogger<BreederController> logger, IBreedingSimulator simulator, IPositionsController positionsController, BeeGenerator beeGenerator)
        {
            _logger = logger;
            _simulator = simulator;
            _positionsController = positionsController;
            _beeGenerator = beeGenerator;
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

            return pool;
        }
    }
}
