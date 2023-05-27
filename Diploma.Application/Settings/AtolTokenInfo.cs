using System.Text.Json.Serialization;
using Diploma.Domain.Converters;

namespace Diploma.Application.Settings;

public class AtolTokenInfo
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    [JsonPropertyName("timestamp")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime LastLogin { get; set; }
}