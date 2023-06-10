using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

/// <summary>
/// Класс, представляющий ставку НДС в элементе фискального чека в формате АТОЛ.
/// </summary>
public class ReceiptVat
{
    /// <summary>
    /// Тип ставки НДС.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Сумма НДС в копейках.
    /// </summary>
    [JsonPropertyName("sum")]
    public decimal Sum { get; set; }
}