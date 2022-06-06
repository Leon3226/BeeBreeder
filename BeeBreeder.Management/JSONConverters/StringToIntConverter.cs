using Newtonsoft.Json;
using System;

namespace BeeBreeder.WebAPI.Serializing.JSONConverters
{
    public class StringToIntConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            if (reader.TokenType == JsonToken.Integer)
                return reader.Value;

            if (reader.TokenType == JsonToken.String)
            {
                if (string.IsNullOrEmpty((string)reader.Value))
                    return null;
                int num;
                //Tenta converter o valor
                if (int.TryParse((string)reader.Value, out num))
                {
                    return num;
                }
                //Retorna 0
                else
                {
                    return 0;
                }

            }
            throw new JsonReaderException(string.Format("Unexcepted token {0}", reader.TokenType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
