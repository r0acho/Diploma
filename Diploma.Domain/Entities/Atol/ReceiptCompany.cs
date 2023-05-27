using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

public class ReceiptCompany
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("sno")]
    public string Sno { get; set; }
    [JsonPropertyName("inn")]
    public string Inn { get; set; }
    [JsonPropertyName("payment_address")]
    public string PaymentAddress { get; set; }
}