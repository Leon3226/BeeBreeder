namespace BeeBreeder.WebAPI.Serializing
{
    public static class LuaToJsonConverterExtensions
    {
        public static string FromLuaToJsonString(this string lua)
        {
            var json =  lua.Replace("=",":");
            json = json.Replace("[","");
            json = json.Replace("},{","},unk:{");
            return json.Replace("]","");
        }
    }
}