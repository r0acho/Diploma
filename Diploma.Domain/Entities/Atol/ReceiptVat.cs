using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

public class ReceiptVat
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("sum")]
    public decimal Sum { get; set; }
}