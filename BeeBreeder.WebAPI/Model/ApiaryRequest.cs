using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BeeBreeder.WebAPI.Model
{
    public class ApiaryRequest
    {
        public string ApiaryIdentifier;
        public List<PlacedBee> List = new();
        
        public static ApiaryRequest FromJson(JObject json)
        {
            var request = new ApiaryRequest();
            var bees = json["list"].Children();
            foreach (var jToken in bees)
            {
                var bee = (JProperty) jToken;
                var modelBee = bee.Value.ToObject<PlacedBee>();
                request.List.Add(modelBee);
            }
            return request;
        }
    }
}