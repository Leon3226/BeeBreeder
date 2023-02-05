using BeeBreeder.Management.Manager;
using BeeBreeder.Management.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeBreeder.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InGameValuesController : ControllerBase
    {
        //private readonly SimpleManager _simpleManager;
        //private readonly UserManager<IdentityUser> _userManager;

        //public InGameValuesController(SimpleManager simpleManager, UserManager<IdentityUser> userManager)
        //{
        //    _userManager = userManager;
        //    _simpleManager = simpleManager;
        //}

        //[HttpGet("{apiaryId}")]
        //public async Task<ActionResult<IEnumerable<Transposer>>> Transposers()
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    if (userId == null)
        //        return Unauthorized();

        //    await _simpleManager.LoadData(new LoadDataParams() { LoadItems = false });
        //    return Ok(_simpleManager.Computers.SelectMany(x => x.Trasposers));
        //}
    }
}
