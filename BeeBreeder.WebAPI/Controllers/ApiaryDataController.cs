using System.Collections.Generic;
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
        public async Task<BeePool> AvaliableBeesAsync()
        {
            var pool = new BeePool
            {
                Bees = new List<BeeStack>
                {
                    new(_beeGenerator.Generate(Species.Forest, Gender.Princess), 8),
                    new(_beeGenerator.Generate(Species.Forest), 8),
                    new(_beeGenerator.Generate(Species.Meadows, Gender.Princess), 8),
                    new(_beeGenerator.Generate(Species.Meadows), 8),
                    new(_beeGenerator.Generate(Species.Steadfast), 1),
                    new(_beeGenerator.Generate(Species.Tropical), 1),
                    new(_beeGenerator.Generate(Species.Modest), 1),
                    new(_beeGenerator.Generate(Species.Modest, Gender.Princess),  1),
                    new(_beeGenerator.Generate(Species.Tropical, Gender.Princess),  1)
                }
            };
                
            _simulator.Pool = pool;
            await Task.Run(() => _simulator.Breed(3000));

            return pool;
        }
    }
}
