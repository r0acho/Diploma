using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

public class Service
{
    [JsonPropertyName("callback_url")]
    public string CallbackUrl { get; set; }
}