using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

public class ReceiptItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }
    [JsonPropertyName("sum")]
    public decimal Sum { get; set; }
    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; }
    [JsonPropertyName("vat")]
    public ReceiptVat Vat { get; set; } = new();
}