using System.Collections.Generic;
using System.Linq;
using BeeBreeder.Common.Model.Bees;
using BeeBreeder.Common.Model.Positioning;
using BeeBreeder.WebAPI.Model;

namespace BeeBreeder.WebAPI.Mapping
{
    public static class ApiaryRequestExtensions
    {
        public static Dictionary<BeeStack, ApiaryPosition> GetModel(this ApiaryRequest request)
        {
            return request.List.ToDictionary(x => x.Bee.ToModelBee(), x => x.BeePosition);
        }
    }
}