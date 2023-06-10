using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Diploma.Domain.Responses;

/// <summary>
/// Класс хранения результатов фискальных операций.
/// </summary>
public record FiscalizeResponse
{
    /// <summary>
    /// Уникальный идентификатор записи в БД.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public ulong Id { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор операции.
    /// </summary>
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; }
    
    /// <summary>
    /// Статус операции.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    /// <summary>
    /// Ошибка, возникшая при выполнении операции.
    /// </summary>
    [JsonPropertyName("error")]
    public string? ErrorJson;
    
    /// <summary>
    /// Время фискализации в формате yyyyMMddHHmmss.
    /// </summary>
    [JsonPropertyName("timestamp")]
    [JsonConverter(typeof(Diploma.Domain.Converters.DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Идентификатор сессии.
    /// </summary>
    public ulong SessionId { get; set; }
}