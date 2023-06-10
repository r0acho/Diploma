using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

/// <summary>
/// Класс, представляющий элемент фискального чека в формате АТОЛ.
/// </summary>
public class ReceiptItem
{
    /// <summary>
    /// Наименование товара/услуги в чеке.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Цена товара/услуги в копейках.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Количество товара/услуги (вес/объем).
    /// </summary>
    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Сумма элемента чека в копейках.
    /// </summary>
    [JsonPropertyName("sum")]
    public decimal Sum { get; set; }

    /// <summary>
    /// Способ оплаты.
    /// </summary>
    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Данные по ставке НДС.
    /// </summary>
    [JsonPropertyName("vat")]
    public ReceiptVat Vat { get; set; } = new();
}