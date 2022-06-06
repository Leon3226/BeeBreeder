using BeeBreeder.Management.Repository;
using BeeBreeder.Property.Model;
using BeeBreeder.Property.Repository;
using BeeBreeder.WebAPI.Model;
using BeeBreeder.WebAPI.Sockets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeeBreeder.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerBindController : ControllerBase
    {
        private readonly IComputerBindRequestRepository _computerBindRequestRepository;
        private readonly IComputerRepository _computerRepository;
        private readonly IGameApiariesDataRepository _gameApiariesDataRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ComputerBindController(IComputerBindRequestRepository computerBindRequestRepository,
            UserManager<IdentityUser> userManager,
            IComputerRepository computerRepository,
            IGameApiariesDataRepository gameApiariesDataRepository)
        {
            _computerBindRequestRepository = computerBindRequestRepository;
            _userManager = userManager;
            _computerRepository = computerRepository;
            _gameApiariesDataRepository = gameApiariesDataRepository;
        }

        //TODO: Return code or smth
        // GET api/<BindComputerController>/5
        [HttpGet("{computerId}")]
        public async Task<ActionResult<ComputerBindResponse>> Get(string computerId)
        {
            var response = new ComputerBindResponse();
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();

            var computer = await _computerRepository.GetComputerAsync(computerId);
            if (computer != null && computer.UserId == userId)
            {
                response.Response = "This computer is already belongs to you";
                return response;
            }
            if (computer != null && computer.UserId != null && computer.UserId != userId)
            {
                response.Response = "This computer is bond to someone else. If it is you, reset your bee breeder identifier";
                return response;
            }

            if (!_gameApiariesDataRepository.IsActive(computerId))
            {
                response.Response = "This computer is disabled or the program is not running or connected";
                return response;
            }

            response.Possible = true;
            return response;
        }

        //TODO: Revision as REST
        // POST api/<BindComputerController>
        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody]ComputerBindRequest request)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return Unauthorized();

            var code = request.ConfirmCode;
            if (string.IsNullOrWhiteSpace(code))
            {
                return await _computerBindRequestRepository.CreateRequestAsync(request, userId, new System.TimeSpan(1,0,0)) == null; //TODO: Review
            }
            else
            {
                return await _computerBindRequestRepository.AssertAsync(request.ComputerIdentifier, userId, code);
            }
        }
    }
}
