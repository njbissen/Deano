using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Deano.Azure
{
    public class TimeSpanConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var timeSpan = (TimeSpan)value;
            var timeSpanString = XmlConvert.ToString(timeSpan);
            serializer.Serialize(writer, timeSpanString);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var value = serializer.Deserialize<string>(reader);
            var timeSpan = XmlConvert.ToTimeSpan(value);

            return timeSpan;
        }

        public override bool CanConvert(Type objectType)
        {
            var canConvert = objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);

            return canConvert;
        }
    }
}
