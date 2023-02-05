using BeeBreeder.Management.Repository;
using BeeBreeder.Property.Model;
using BeeBreeder.Property.Repository;
using BeeBreeder.WebAPI.Sockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeeBreeder.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiariesController : ControllerBase
    {
        private readonly IApiaryRepository _apiaryDataRepository;
        private readonly IComputerRepository _computerRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IGameApiariesDataRepository _gameApiariesDataRepository;


        public ApiariesController(IApiaryRepository apiaryDataRepository,
            IComputerRepository computerRepository,
            UserManager<IdentityUser> userManager,
            IGameApiariesDataRepository gameApiariesDataRepository)
        {
            _apiaryDataRepository = apiaryDataRepository;
            _computerRepository = computerRepository;
            _userManager = userManager;
            _gameApiariesDataRepository = gameApiariesDataRepository;
        }

        // GET: api/<ApiariesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apiary>>> Get()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            return await _apiaryDataRepository.GetApiariesAsync(userId);
        }

        // GET api/<ApiariesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Apiary>> Get(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            var apiary = await _apiaryDataRepository.GetApiaryAsync(userId, id);
            return apiary;
        }

        // GET api/<ApiariesController>/5
        [HttpPost("{apiaryId}/{computerId}")]
        public async Task<ActionResult<bool>> AttachComputer(int apiaryId, int computerId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();

            var computer = await _computerRepository.GetComputerAsync(userId, computerId);
            if (computer.UserId != userId)
                return false;

            var apiary = await _apiaryDataRepository.GetApiaryAsync(userId, apiaryId);
            if (apiary == null)
            {
                return false;
            }

            await _computerRepository.SetApiary(computerId, apiaryId);
            return true;
        }

        // GET api/<ApiariesController>/5
        [HttpDelete("{apiaryId}/{computerId}")]
        public async Task<ActionResult<bool>> DetachComputer(int apiaryId, int computerId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();

            var computer = await _computerRepository.GetComputerAsync(userId, computerId);
            if (computer.UserId != userId)
                return false;

            var apiary = await _apiaryDataRepository.GetApiaryAsync(userId, apiaryId);
            if (apiary == null)
            {
                return false;
            }

            await _computerRepository.DetachApiary(computerId);
            return true;
        }

        // POST api/<ApiariesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Apiary value)
        {
            var userId = _userManager.GetUserId(HttpContext.User);  
            if (userId == null)
                return Unauthorized();
            await _apiaryDataRepository.AddApiaryAsync(userId, value);
            return Ok();
        }

        // POST api/<ApiariesController>
        [HttpPost("computers/{identifier}")]
        public async Task<ActionResult> Post([FromBody] string value)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();

            //await _computerRepository.SetApiary(userId, value);
            return Ok();
        }

        // PUT api/<ApiariesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]Apiary value)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            value.Id = id;
            await _apiaryDataRepository.UpdateApiaryAsync(value);
            return Ok();
        }

        // DELETE api/<ApiariesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            await _apiaryDataRepository.DeleteApiaryAsync(id);
            return Ok();
        }
    }
}
