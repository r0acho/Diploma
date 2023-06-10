using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

/// <summary>
/// Класс, представляющий данные клиента в фискальном чеке.
/// </summary>
public class ReceiptClient
{
    /// <summary>
    /// Адрес электронной почты клиента.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
}