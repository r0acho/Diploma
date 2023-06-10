using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

/// <summary>
/// Класс, представляющий данные фискального чека в формате АТОЛ.
/// </summary>
public class ReceiptData
{
    /// <summary>
    /// Уникальный внешний идентификатор чека.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string ExternalId => Guid.NewGuid().ToString("N");

    /// <summary>
    /// Фискальный чек.
    /// </summary>
    [JsonPropertyName("receipt")]
    public Receipt Receipt { get; set; } = new();

    /// <summary>
    /// Сервис, через который отправляется чек.
    /// </summary>
    [JsonPropertyName("service")]
    public Service Service { get; set; } = new();

    /// <summary>
    /// Временная метка создания чека.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
}