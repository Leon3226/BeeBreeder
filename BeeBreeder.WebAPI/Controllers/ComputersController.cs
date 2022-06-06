using AutoMapper;
using BeeBreeder.Management.Repository;
using BeeBreeder.Property.Model;
using BeeBreeder.Property.Repository;
using BeeBreeder.WebAPI.Management;
using BeeBreeder.WebAPI.Model;
using BeeBreeder.WebAPI.Sockets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeeBreeder.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        private readonly IComputerRepository _computerRepository;
        private readonly IGameApiariesDataRepository _gameApiariesDataRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ComputersController(IComputerRepository computerRepository,
            UserManager<IdentityUser> userManager,
            IGameApiariesDataRepository gameApiariesDataRepository)
        {
            _computerRepository = computerRepository;
            _userManager = userManager;
            _gameApiariesDataRepository = gameApiariesDataRepository;
        }

        // GET: api/<ApiariesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Computer>>> Get()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            return (await _computerRepository.GetComputersAsync(userId)).Select(x => new Computer
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Identifier = x.Identifier,
                Active = _gameApiariesDataRepository.IsActive(x.Identifier)
            }).ToArray();
        }

        // GET api/<ApiariesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> Get(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            var dbComputer = await _computerRepository.GetComputerAsync(userId, id);
            return new Computer
            {
                Id = dbComputer.Id,
                Name = dbComputer.Name,
                Description = dbComputer.Description,
                Identifier = dbComputer.Identifier,
                Active = _gameApiariesDataRepository.IsActive(dbComputer.Identifier)
            };
        }

        // POST api/<ApiariesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Computer value)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            await _computerRepository.AddComputerAsync(userId, new ApiaryComputer
            {
                Description = value.Description,
                Identifier = value.Identifier,
                Name = value.Name,
                Id = value.Id
            });
            return Ok();
        }

        // PUT api/<ApiariesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]Computer value)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            value.Id = id;
            await _computerRepository.UpdateComputerAsync(new ApiaryComputer
            {
                Description = value.Description,
                Identifier = value.Identifier,
                Name = value.Name,
                Id = value.Id
            });
            return Ok();
        }

        // DELETE api/<ApiariesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            await _computerRepository.DeleteComputerAsync(id);
            return Ok();
        }
    }
}
