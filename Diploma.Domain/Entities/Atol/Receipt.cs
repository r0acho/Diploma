using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

/// <summary>
/// Класс, представляющий фискальный чек.
/// </summary>
public class Receipt
{
    /// <summary>
    /// Данные клиента.
    /// </summary>
    [JsonPropertyName("client")]
    public ReceiptClient Client { get; set; } = new();

    /// <summary>
    /// Данные компании.
    /// </summary>
    [JsonPropertyName("company")]
    public ReceiptCompany Company { get; set; } = new();

    /// <summary>
    /// Элементы чека.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ReceiptItem> Items { get; set; } = new();

    /// <summary>
    /// Список платежей.
    /// </summary>
    [JsonPropertyName("payments")]
    public List<ReceiptPayment> Payments { get; set; } = new();

    /// <summary>
    /// Итоговая сумма чека.
    /// </summary>
    [JsonPropertyName("total")]
    public decimal Total { get; set; }

    /// <summary>
    /// ФИО кассира.
    /// </summary>
    [JsonPropertyName("cashier")]
    public string Cashier { get; set; }
}