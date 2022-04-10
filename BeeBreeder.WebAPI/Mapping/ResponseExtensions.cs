using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeBreeder.Common.Model.Positioning;
using BeeBreeder.WebAPI.Model;

namespace BeeBreeder.WebAPI.Mapping
{
    //TODO: Replace all this shit with the JSON serialization
    public static class ResponseExtensions
    {
        public static string LuaSerialize(this BreedResponse response)
        {
            return $@"{{flush={response.Flush.LuaSerialize()},breed={response.Breed.LuaSerialize()}}}";
        }
        public static string LuaSerialize(this (ApiaryPosition, ApiaryPosition) beePositions)
        {
            return $@"{{princess={beePositions.Item1.LuaSerialize()},drone={beePositions.Item2.LuaSerialize()}}}";
        }
        
        public static string LuaSerialize(this IEnumerable<(ApiaryPosition, ApiaryPosition)> beePositions)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            foreach (var position in beePositions)
            {
                sb.Append(position.LuaSerialize());
                sb.Append(",");
            }

            if (beePositions.Any()) sb.Remove(sb.Length - 1, 1);
            sb.Append("}");

            return sb.ToString();
        }
        
        public static string LuaSerialize(this IEnumerable<ApiaryPosition> beePositions)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            foreach (var position in beePositions)
            {
                sb.Append(position.LuaSerialize());
                sb.Append(",");
            }

            if (beePositions.Any()) sb.Remove(sb.Length - 1, 1);
            sb.Append("}");

            return sb.ToString();
        }
        
        public static string LuaSerialize(this ApiaryPosition position)
        {
            return $@"{{position={{trans=""{position.Trans}"",side={position.Side},slot={position.Slot}}}}}";
        }
    }
}