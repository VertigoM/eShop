using System.Text.Json;
using System.Text.Json.Serialization;

namespace eShop.API.Entities
{
    internal sealed class JsonToByteArrayConverter : JsonConverter<byte[]?>
    {
        public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!reader.TryGetBytesFromBase64(out byte[]? result) || result == default)
            {
                throw new Exception("Failed to parse");
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, byte[]? value, JsonSerializerOptions options)
        {
            writer.WriteBase64StringValue(value);
        }
    }
}
