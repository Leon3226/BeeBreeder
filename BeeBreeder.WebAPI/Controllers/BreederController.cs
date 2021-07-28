using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Breeder;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.WebAPI.Mapping;
using BeeBreeder.WebAPI.Model;
using BeeBreeder.WebAPI.Serializing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BeeBreeder.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreederController : Controller
    {
        private readonly ILogger<BreederController> _logger;
        private readonly IBeeBreeder _breeder;

        public BreederController(ILogger<BreederController> logger, IBeeBreeder breeder)
        {
            _logger = logger;
            _breeder = breeder;
        }
        // GET
        [HttpPost]
        public async Task<string> Post(string inv)
        {   
            string json;
            using (var reader = new StreamReader(Request.Body))
            {
                json = (await reader.ReadToEndAsync()).FromLuaToJsonString();
            }

            dynamic data = JsonConvert.DeserializeObject(json);
            ApiaryRequest data4 = ApiaryRequest.FromJson((JObject)data);
            var tb = data4.GetModel();
            _breeder.Pool = new BeePool() {Bees = tb.Keys.ToList()};
            var toFlush = _breeder.ToFlush().ToArray();
            var flush = toFlush.Select(x => tb[x]);
            _breeder.Pool.Bees = _breeder.Pool.Bees.Except(toFlush).ToList();
            var toBreed = _breeder.GetBreedingPairs();
            var breedPositions = toBreed.Select(x => (tb[x.Item1], tb[x.Item2]));
             
            _logger.Log(LogLevel.Information, json);
            Console.WriteLine(json);
            var response = new BreedResponse() {Breed = breedPositions.ToList(), Flush = flush.ToList()};
            return response.LuaSerialize();
        }
    }
}