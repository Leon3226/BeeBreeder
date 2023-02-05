using AutoMapper;
using BeeBreeder.Management.Repository;
using BeeBreeder.Property.Model;
using BeeBreeder.Property.Repository;
using BeeBreeder.WebAPI.Model;
using BeeBreeder.WebAPI.Sockets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeBreeder.WebAPI.Controllers
{
    //TODO: Add check for computer user property
    [Route("api/computers/{computerId}/transposers")]
    [ApiController]
    public class TransposersController : ControllerBase
    {
        private readonly ITransposerRepository _transposerRepository;
        private readonly IComputerRepository _computerRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public TransposersController(ITransposerRepository transposerRepository,
            IComputerRepository computerRepository,
            UserManager<IdentityUser> userManager)
        {
            _transposerRepository = transposerRepository;
            _computerRepository = computerRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transposer>>> Get(int computerId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            return (await _transposerRepository.GetTransposersAsync(computerId)).ToArray();
        }

        //[HttpPost("load")]
        //public async Task<ActionResult<IEnumerable<Transposer>>> Load(int computerId)
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    if (userId == null)
        //        return Unauthorized();
        //    var computer = await _computerRepository.GetComputerAsync(userId, computerId);
        //    if (computer != null)
        //    {
        //        computer.
        //    }
        //    return (await _transposerRepository.GetTransposersAsync(computerId)).ToArray();
        //}

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Transposer value, int computerId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            await _transposerRepository.AddTransposerAsync(computerId, value);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, int computerId, [FromBody] Transposer value)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            value.Id = id;
            await _transposerRepository.UpdateTransposerAsync(value, id, computerId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            await _transposerRepository.DeleteTransposerAsync(id);
            return Ok();
        }

    }
}
