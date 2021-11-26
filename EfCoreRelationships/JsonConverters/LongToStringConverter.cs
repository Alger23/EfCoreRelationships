using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EfCoreRelationships.JsonConverters
{
    public class LongToStringConverter : JsonConverter<long>
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(System.Int64).Equals(objectType);
        }

        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                // try to parse number directly from bytes
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out long number, out int bytesConsumed) && span.Length == bytesConsumed)
                    return number;

                // try to parse from a string if the above failed, this covers cases with other escaped/UTF characters
                if (Int64.TryParse(reader.GetString(), out number))
                    return number;
            }

            // fallback to default handling
            return reader.GetInt64();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Convert.ToString(value));
        }
    }
}