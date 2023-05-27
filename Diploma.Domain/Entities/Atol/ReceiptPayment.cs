using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

public class ReceiptPayment
{
    [JsonPropertyName("type")]
    public int Type { get; set; }
    [JsonPropertyName("sum")]
    public decimal Sum { get; set; }
}