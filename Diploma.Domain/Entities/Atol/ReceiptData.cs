using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

public class ReceiptData
{
    [JsonPropertyName("external_id")] public string ExternalId => Guid.NewGuid().ToString("N");
    [JsonPropertyName("receipt")]
    public Receipt Receipt { get; set; } = new();
    [JsonPropertyName("service")]
    public Service Service { get; set; } = new();
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
}