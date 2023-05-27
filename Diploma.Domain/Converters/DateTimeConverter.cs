using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Diploma.Domain.Converters;


public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "dd.MM.yyyy HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string dateString = reader.GetString();
        DateTime dt = DateTime.ParseExact(dateString, DateFormat, CultureInfo.InvariantCulture);

        return dt;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat));
    }
}