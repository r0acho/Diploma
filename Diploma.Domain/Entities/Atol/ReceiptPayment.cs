using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

/// <summary>
/// Класс, представляющий платеж в фискальном чеке в формате АТОЛ.
/// </summary>
public class ReceiptPayment
{
    /// <summary>
    /// Тип платежа.
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; set; }

    /// <summary>
    /// Сумма платежа в копейках.
    /// </summary>
    [JsonPropertyName("sum")]
    public decimal Sum { get; set; }
}