using BeeBreeder.Common.Data;
using BeeBreeder.WebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BeeBreeder.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModsController : ControllerBase
    {
        private readonly IModsProvider _modsProvider;

        public ModsController(IModsProvider modsProvider)
        {
            _modsProvider = modsProvider;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Mod>> Get()
        {
            return Ok(_modsProvider.AllAvaliableMods());
        }
    }
}
