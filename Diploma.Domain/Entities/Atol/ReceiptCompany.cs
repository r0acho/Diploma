using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

/// <summary>
/// Класс, представляющий данные компании в фискальном чеке.
/// </summary>
public class ReceiptCompany
{
    /// <summary>
    /// Адрес электронной почты компании.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// Система налогообложения.
    /// </summary>
    [JsonPropertyName("sno")]
    public string Sno { get; set; }

    /// <summary>
    /// ИНН компании.
    /// </summary>
    [JsonPropertyName("inn")]
    public string Inn { get; set; }

    /// <summary>
    /// Адрес места расчетов.
    /// </summary>
    [JsonPropertyName("payment_address")]
    public string PaymentAddress { get; set; }
}