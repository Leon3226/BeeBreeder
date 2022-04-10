using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Breeding.Flusher;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.WebAPI.Mapping;
using BeeBreeder.WebAPI.Model;
using BeeBreeder.WebAPI.Serializing;
using Microsoft.AspNetCore.Http;
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
        private readonly IBreedAnalyzer _breedAnalyzer;
        private readonly IBreedFlusher _breedFlusher;

        public static BeePool McPool = new BeePool();

        public BreederController(ILogger<BreederController> logger, IBreedAnalyzer breedAnalyzer, IBreedFlusher breedFlusher)
        {
            _logger = logger;
            _breedAnalyzer = breedAnalyzer;
            _breedFlusher = breedFlusher;
        }

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
            var pool = new BeePool() {Bees = tb.Keys.ToList()};
            McPool = pool;
            var toFlush = _breedFlusher.ToFlush(pool).ToArray();
            var flush = toFlush.Select(x => tb[x]);
            var toBreed = _breedAnalyzer.GetBreedingPairs(pool);
            var breedPositions = toBreed.Select(x => (tb.FirstOrDefault(bee => bee.Key.Bee == x.Item1).Value, tb.FirstOrDefault(bee => bee.Key.Bee == x.Item2).Value));
             
            _logger.Log(LogLevel.Information, json);
            Console.WriteLine(json);
            var response = new BreedResponse() {Breed = breedPositions.ToList(), Flush = flush.ToList()};
            return response.LuaSerialize();
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}