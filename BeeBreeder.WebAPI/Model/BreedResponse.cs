using System.Collections.Generic;
using BeeBreeder.Common.Model.Positioning;

namespace BeeBreeder.WebAPI.Model
{
    public class BreedResponse
    {
        public List<ApiaryPosition> Flush;
        public List<(ApiaryPosition, ApiaryPosition)> Breed;
    }
}