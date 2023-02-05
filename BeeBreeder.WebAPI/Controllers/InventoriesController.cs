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
    [Route("api/computers/{computerId}/transposers/{transposerId}/inventories")]
    [ApiController]
    public class InventorysController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public InventorysController(IInventoryRepository inventoryRepository,
            UserManager<IdentityUser> userManager)
        {
            _inventoryRepository = inventoryRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetAll(string transposerId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            return (await _inventoryRepository.GetInventoriesAsync(transposerId)).ToArray();
        }

        [HttpGet("{side}")]
        public async Task<ActionResult<Inventory>> Get(string transposerId, int side)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            return await _inventoryRepository.GetInventoryAsync(transposerId, side);
        }

        //[HttpPost("load")]
        //public async Task<ActionResult<IEnumerable<Inventory>>> Load(int computerId)
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    if (userId == null)
        //        return Unauthorized();
        //    var computer = await _computerRepository.GetComputerAsync(userId, computerId);
        //    if (computer != null)
        //    {
        //        computer.
        //    }
        //    return (await _inventoryRepository.GetInventorysAsync(computerId)).ToArray();
        //}

        [HttpPost("{side}")]
        public async Task<ActionResult> Post([FromBody] Inventory value, string transposerId, int side)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            await _inventoryRepository.AddInventoryAsync(transposerId, side, value);
            return Ok();
        }

        [HttpPut("{side}")]
        public async Task<ActionResult> Put(string transposerId, int side, [FromBody] Inventory value)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            await _inventoryRepository.UpdateInventoryAsync(transposerId, side, value);
            return Ok();
        }

        [HttpDelete("{side}")]
        public async Task<ActionResult> Delete(string transposerId, int side)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();
            await _inventoryRepository.DeleteInventoryAsync(transposerId, side);
            return Ok();
        }

    }
}
