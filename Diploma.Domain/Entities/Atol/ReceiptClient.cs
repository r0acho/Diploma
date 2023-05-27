using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

public class ReceiptClient
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
}