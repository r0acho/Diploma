using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

public class Receipt
{
    [JsonPropertyName("client")] public ReceiptClient Client { get; set; } = new();

    [JsonPropertyName("company")] public ReceiptCompany Company { get; set; } = new();

    [JsonPropertyName("items")] public List<ReceiptItem> Items { get; set; } = new();

    [JsonPropertyName("payments")] 
    public List<ReceiptPayment> Payments { get; set; } = new();
    [JsonPropertyName("total")]
    public decimal Total { get; set; }
    [JsonPropertyName("cashier")]
    public string Cashier { get; set; }
}